using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using InventarioA.Api.Data;
using InventarioA.Api.Models;
using InventarioA.Api.Events;


namespace InventarioA.Api.Services
{
    public class RabbitMQConsumer : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<RabbitMQConsumer> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private IConnection? _connection;
        private IChannel? _channel;

        public RabbitMQConsumer(
            IConfiguration configuration,
            ILogger<RabbitMQConsumer> logger,
            IServiceScopeFactory scopeFactory
            )
        {
            _configuration = configuration;
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(
            CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory
            {
                HostName = _configuration["RabbitMQ:HostName"],
                Port = int.Parse(_configuration["RabbitMQ:Port"]!),
                UserName = _configuration["RabbitMQ:UserName"],
                Password = _configuration["RabbitMQ:Password"]
            };

            _connection = await factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();

            var queueName = _configuration["RabbitMQ:QueueName"]!;

            await _channel.QueueDeclareAsync(
                queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.ReceivedAsync += async (sender, ea) =>
            {
                var body = ea.Body.ToArray();

                var mensaje = Encoding.UTF8.GetString(body);

                var evento =
                    JsonSerializer.Deserialize<LibroCreadoEvento>(mensaje);

                if (evento != null)
                {
                    _logger.LogInformation(
                        "Libro creado recibido. IdLibro: {IdLibro}",
                        evento.IdLibro
                    );

                    using var scope = _scopeFactory.CreateScope();

                    var dbContext =
                        scope.ServiceProvider
                            .GetRequiredService<InventarioDBContext>();

                    var existe = await dbContext.Inventarios
                        .AnyAsync(i => i.IdLibro == evento.IdLibro);

                    if (!existe)
                    {
                        var inventario = new Inventario
                        {
                            IdLibro = evento.IdLibro,
                            Stock = 0,
                            StockMinimo = 0,
                            Ubicacion = "Sin asignar"
                        };

                        dbContext.Inventarios.Add(inventario);

                        await dbContext.SaveChangesAsync();

                        _logger.LogInformation(
                            "Inventario creado automáticamente para IdLibro: {IdLibro}",
                            evento.IdLibro
                        );
                    }
                }

                await _channel.BasicAckAsync(
                    deliveryTag: ea.DeliveryTag,
                    multiple: false
                );
            };

            await _channel.BasicConsumeAsync(
                queue: queueName,
                autoAck: false,
                consumer: consumer
            );

            await Task.Delay(
                Timeout.Infinite,
                stoppingToken
            );
        }

    }
}
