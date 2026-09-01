using Microsoft.EntityFrameworkCore;

namespace CategoriaA.Api.Data
{
    public class CategoriasDBContext : DbContext
    {
        public CategoriasDBContext(DbContextOptions<CategoriasDBContext> options) : base(options)
        {
        }
        public DbSet<Models.Categorias> Categorias { get; set; } = null!;
    }
}
