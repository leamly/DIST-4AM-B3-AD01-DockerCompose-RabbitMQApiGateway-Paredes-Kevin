using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibroA.Api.Models
{
    // Nombre de la tabla en la base de datos
    // Se utiliza el atributo Table para especificar el nombre de la tabla
    [Table("Libros")]
    public class Libros
    {
        // Clave primaria de la tabla
        // La propiedad es el atributo + los getter y setter
        // Esto es una convención de C# para definir propiedades de una clase
        [Key]
        [Column("IdLibro")]
        public int IdLibro { get; set; }


        [StringLength(150)]
        [Column("Titulo_libro")]
        public string Titulo { get; set; } = string.Empty;


        [StringLength(100)]
        [Column("Autor_libro")]
        public string Autor { get; set; } = string.Empty;
        

        [StringLength(100)]
        [Column("Isbn_libro")]
        public string Isbn { get; set; } = string.Empty;
        

        [Column("Precio_libro", TypeName = "decimal(10,2)")]
        public decimal Precio { get; set; }
        

        [Column("Estado_libro")]
        public bool Estado { get; set; }
    }
}
