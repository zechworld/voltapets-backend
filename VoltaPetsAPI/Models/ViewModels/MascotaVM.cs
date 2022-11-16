using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace VoltaPetsAPI.Models.ViewModels
{
    public class MascotaVM
    {
        [Required]
        [MaxLength(20)]
        public string Nombre { get; set; }

        [Column("descripcion")]
        [StringLength(500)]
        public string? Descripcion { get; set; }

        [Column("fecha_nacimiento")]
        [Required]
        public DateTime FechaNacimiento { get; set; }

        [Column("esterilizado")]
        [Required]
        public bool Esterilizado { get; set; }

        [Column("edad_registro")]
        [Required]
        public int EdadRegistro { get; set; }

        //FK Tutor
        [Column("codigo_tutor")]
        [Required]
        public int CodigoTutor { get; set; }

        [ForeignKey("CodigoTutor")]
        public virtual Tutor Tutor { get; set; }

        //FK Sexo
        [Column("codigo_sexo")]
        [Required]
        public int CodigoSexo { get; set; }

        [ForeignKey("CodigoSexo")]
        public virtual Sexo Sexo { get; set; }

        //FK Tamanio
        [Column("codigo_tamanio")]
        [Required]
        public int CodigoTamanio { get; set; }

        [ForeignKey("CodigoTamanio")]
        public virtual Tamanio Tamanio { get; set; }

        //FK Raza
        [Column("codigo_raza")]
        [Required]
        public int CodigoRaza { get; set; }

        [ForeignKey("CodigoRaza")]
        public virtual Raza Raza { get; set; }

        //FK Grupo Etario
        [Column("codigo_etario")]
        [Required]
        public int CodigoEtario { get; set; }

        [ForeignKey("CodigoEtario")]
        public virtual GrupoEtario GrupoEtario { get; set; }

        //FK Imagen
        [Column("codigo_imagen")]
        public int? CodigoImagen { get; set; }

        [ForeignKey("CodigoImagen")]
        public virtual Imagen Imagen { get; set; }

        //FK Estado Mascota
        [Column("codigo_estado_mascota")]
        [Required]
        public int CodigoEstadoMascota { get; set; }

        [ForeignKey("CodigoEstadoMascota")]
        public virtual EstadoMascota EstadoMascota { get; set; }
    }
}
