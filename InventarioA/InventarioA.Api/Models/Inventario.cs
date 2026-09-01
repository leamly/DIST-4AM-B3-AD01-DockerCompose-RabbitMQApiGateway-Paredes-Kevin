using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventarioA.Api.Models
{
    [Table("Inventarios")]
    public class Inventario
    {
        [Key]
        [Column("IdInventario")]
        public int IdInventario { get; set; }

        [Column("IdLibro")]
        public int IdLibro { get; set; }


        [Column("Stock_inventario")]
        public int Stock { get; set; }


        [Column("StockMinimo_inventario")]
        public int StockMinimo { get; set; }


        [Column("Ubicacion_inventario")]
        [MaxLength(40)]
        public string Ubicacion { get; set; }



    }
}
