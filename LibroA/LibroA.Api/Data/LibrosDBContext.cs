using Microsoft.EntityFrameworkCore;

namespace LibroA.Api.Data
{
    public class LibrosDBContext : DbContext
    {
        // Constructor de la clase LibrosDBContext 
        // Se utiliza el constructor base de la clase DbContext para pasar las opciones de configuración
        // Entity Framework Core utiliza este constructor para configurar la conexión a la base de datos y otras opciones relacionadas con el contexto de datos.
        // El DbSet en .NET Core es una colección de entidades que se mapea a una tabla en la base de datos. En este caso, el DbSet<LibroA.Api.Models.Libros> representa la tabla "Libros" en la base de datos y permite realizar operaciones CRUD (Crear, Leer, Actualizar, Eliminar) sobre los registros de esa tabla.
        public LibrosDBContext(DbContextOptions<LibrosDBContext> options) : base(options)
        {
        }

        // Propiedad que representa la tabla "Libros" en la base de datos
        public DbSet<LibroA.Api.Models.Libros> Libros { get; set; }


    }
}
