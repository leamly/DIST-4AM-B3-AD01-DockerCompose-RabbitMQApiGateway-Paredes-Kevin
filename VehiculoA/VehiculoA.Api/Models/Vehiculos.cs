using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehiculoA.Api.Models
{
    [Table("Vehiculos")]
    public class Vehiculos
    {
        [Key]
        [Column("IdVehiculo")]
        public int IdVehiculo { get; set; }


        [Column("IdCategoria")]
        public int IdCategoria { get; set; }


        [Column("Marca")]
        public string Marca { get; set; } = string.Empty;


        [Column("Modelo")] 
        public string Modelo { get; set; } = string.Empty;


        [Column("Precio")]
        public decimal Precio { get; set; }


        [Column("Stock")]
        public int Stock { get; set; }


        [Column("Estado")]
        public bool Estado { get; set; }

    }
}
