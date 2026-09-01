using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using VehiculoA.Api.Data;
using VehiculoA.Api.Events;
using VehiculoA.Api.Models;

namespace VehiculoA.Api.Services
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
                    JsonSerializer.Deserialize<CategoriaCreadoEvento>(mensaje);

                if (evento != null)
                {
                    _logger.LogInformation(
                        "Categoria creada recibida. IdCategoria: {IdCategoria}",
                        evento.IdCategoria
                    );

                    using var scope = _scopeFactory.CreateScope();

                    var dbContext =
                        scope.ServiceProvider
                            .GetRequiredService<VehiculosDBContext>();

                    var existe = await dbContext.Vehiculos
                        .AnyAsync(i => i.IdCategoria == evento.IdCategoria);

                    if (!existe)
                    {
                        var Vehiculos = new Vehiculos
                        {
                            IdCategoria = evento.IdCategoria,
                            Marca = "Sin marca",
                            Modelo = "Sin modelo",
                            Precio = 0,
                            Stock = 0,
                        };

                        dbContext.Vehiculos.Add(Vehiculos);

                        await dbContext.SaveChangesAsync();

                        _logger.LogInformation(
                            "Vehiculo creado automáticamente para IdCategoria: {IdCategoria}",
                            evento.IdCategoria
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
