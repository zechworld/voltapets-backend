using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VoltaPetsAPI.Models
{
    [Table("parque_pet_friendly")]
    public class ParquePetFriendly
    {
        [Column("codigo_parque")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("descripcion")]
        [Required]
        [StringLength(50)]
        public string Descripcion { get; set; }

        [Column("latitud")]
        [Required]
        public double Latitud { get; set; }

        [Column("longitud")]
        [Required]
        public double Longitud { get; set; }

        //Fk Comuna
        [Column("codigo_comuna")]
        [Required]
        public int CodigoComuna { get; set; }

        [ForeignKey("CodigoComuna")]
        public Comuna Comuna { get; set; }

        //Relacion con Paseo
        public ICollection<Paseo> Paseos { get; set; }

    }
}
