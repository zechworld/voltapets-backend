using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace VoltaPetsAPI.Models
{
    [Table("ponderacion")]
    public class Ponderacion
    {
        [Column("codigo_ponderacion")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("positivo")]
        [Required]
        public float Positivo { get; set; }

        [Column("negativo")]
        [Required]
        public float Negativo { get; set; }

        [Column("obediencia")]
        [Required]
        public float Obediencia { get; set; }

        [Column("agresion")]
        [Required]
        public float Agresion { get; set; }

        [Column("fecha_registro")]
        [Required]
        public DateTime FechaRegistro { get; set; }

        [Column("fecha_termino")]
        public DateTime? FechaTermino { get; set; }

        //Relacion con Comportamiento
        public ICollection<Comportamiento> Comportamientos { get; set; }

    }
}
