using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CategoriaA.Api.Models
{
    [Table("Categorias")]
    public class Categorias
    {
        [Key]
        [Column("IdCategoria")]
        public int IdCategoria { get; set; }

        [Column("Nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Column("Descripcion")]
        public string Descripcion { get; set; } = string.Empty;

        [Column("Estado")]
        public bool Estado { get; set; }
    }
}
