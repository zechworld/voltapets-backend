﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VoltaPetsAPI.Models
{
    [Table("comportamiento")]
    public class Comportamiento
    {
        [Column("codigo_comportamiento")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CodigoComportamiento { get; set; }

        [Column("nota_positivo")]
        [Required]
        public float NotaPositivo { get; set; }

        [Column("nota_negativo")]
        [Required]
        public float NotaNegativo { get; set; }

        [Column("nota_obediencia")]
        [Required]
        public float NotaObediencia { get; set; }

        [Column("nota_agresion")]
        [Required]
        public float NotaAgresion { get; set; }

        //FK Ponderacion
        [Column("codigo_ponderacion")]
        [Required]
        public int CodigoPonderacion { get; set; }

        [ForeignKey("CodigoPonderacion")]
        public virtual Ponderacion Ponderacion { get; set; }

        //FK Paseo Mascota
        [Column("codigo_paseo_mascota")]
        [Required]
        public int CodigoPaseoMascota { get; set; }

        [ForeignKey("CodigoPaseoMascota")]
        public virtual PaseoMascota PaseoMascota { get; set; }

    }
}