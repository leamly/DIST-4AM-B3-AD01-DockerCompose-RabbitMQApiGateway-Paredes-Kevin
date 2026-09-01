using Microsoft.EntityFrameworkCore;
using InventarioA.Api.Models;

namespace InventarioA.Api.Data
{
    public class InventarioDBContext : DbContext
    {
        public InventarioDBContext(DbContextOptions<InventarioDBContext> options) : base(options)
        {
        }

        public DbSet<Inventario> Inventarios { get; set; }

        
    }
}
