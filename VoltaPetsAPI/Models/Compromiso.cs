using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VoltaPetsAPI.Models
{
    [Table("compromiso")]
    public class Compromiso
    {
        [Column("codigo_compromiso")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CodigoCompromiso { get; set; }

        [Column("titulo")]
        [Required]
        [StringLength(100)]
        public string Titulo { get; set; }

        [Column("fecha_compromiso")]
        [Required]
        public DateTime FechaCompromiso { get; set; }

        //FK Mascota
        [Column("codigo_mascota")]
        [Required]
        public int CodigoMascota { get; set; }

        [ForeignKey("CodigoMascota")]
        public virtual Mascota Mascota { get; set; }

    }
}
