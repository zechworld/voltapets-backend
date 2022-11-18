using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace VoltaPetsAPI.Models
{
    [Table("paseo_mascota")]
    public class PaseoMascota
    {
        [Column("codigo_paseo_mascota")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("evaluado")]
        [Required]
        public bool Evaluado { get; set; }

        //FK Paseo
        [Column("codigo_paseo")]
        [Required]
        public int CodigoPaseo { get; set; }

        [ForeignKey("CodigoPaseo")]
        public Paseo Paseo { get; set; }

        //FK Mascota
        [Column("codigo_mascota")]
        [Required]
        public int CodigoMascota { get; set; }

        [ForeignKey("CodigoMascota")]
        public Mascota Mascota { get; set; }

        //Relacion 1 a 1 con PuntajePersonalidad
        public PuntajePersonalidad PuntajePersonalidad { get; set; }

        //Relacion 1 a 1 con Comportamiento
        public Comportamiento Comportamiento { get; set; }

    }
}
