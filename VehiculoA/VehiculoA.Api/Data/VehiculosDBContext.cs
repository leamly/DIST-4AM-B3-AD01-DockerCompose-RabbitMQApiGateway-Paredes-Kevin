using Microsoft.EntityFrameworkCore;

namespace VehiculoA.Api.Data
{
    public class VehiculosDBContext : DbContext
    {
        public VehiculosDBContext(DbContextOptions<VehiculosDBContext> options) : base(options)
        {
        }
        public DbSet<VehiculoA.Api.Models.Vehiculos> Vehiculos { get; set; }
    }
}
