
using InventarioA.Api.Data;
using InventarioA.Api.Services;
using Microsoft.EntityFrameworkCore;

namespace InventarioA.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddHostedService<RabbitMQConsumer>();


            builder.Services.AddDbContext<InventarioDBContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("InventarioConnection")));


            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
