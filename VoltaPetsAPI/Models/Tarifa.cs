using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VoltaPetsAPI.Models
{
    [Table("tarifa")]
    public class Tarifa
    {
        [Column("codigo_tarifa")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("basico")]
        [Required]
        public int Basico { get; set; }

        [Column("juego")]
        [Required]
        public int Juego { get; set; }

        [Column("social")]
        [Required]
        public int Social { get; set; }

        [Column("fecha_registro")]
        [Required]
        public DateTime FechaRegistro { get; set; }

        [Column("fecha_termino")]
        public DateTime? FechaTermino { get; set; }

        //FK Paseador
        [Column("codigo_paseador")]
        [Required]
        public int CodigoPaseador { get; set; }

        [ForeignKey("CodigoPaseador")]
        public virtual Paseador Paseador { get; set; }

    }
}
