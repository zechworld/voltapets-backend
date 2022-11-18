using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VoltaPetsAPI.Models
{
    [Table("mascota")]
    public class Mascota
    {
        [Column("codigo_mascota")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("nombre")]
        [Required]
        [StringLength(20)]
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
        public double? EdadRegistro { get; set; }

        //FK Tutor
        [Column("codigo_tutor")]
        [Required]
        public int CodigoTutor { get; set; }

        [ForeignKey("CodigoTutor")]
        public Tutor Tutor { get; set; }

        //FK Sexo
        [Column("codigo_sexo")]
        [Required]
        public int CodigoSexo { get; set; }

        [ForeignKey("CodigoSexo")]
        public Sexo Sexo { get; set; }

        //FK Tamanio
        [Column("codigo_tamanio")]
        [Required]
        public int CodigoTamanio { get; set; }

        [ForeignKey("CodigoTamanio")]
        public Tamanio Tamanio { get; set; }

        //FK Raza
        [Column("codigo_raza")]
        [Required]
        public int CodigoRaza { get; set; }

        [ForeignKey("CodigoRaza")]
        public Raza Raza { get; set; }

        //FK Grupo Etario
        [Column("codigo_etario")]
        [Required]
        public int CodigoEtario { get; set; }

        [ForeignKey("CodigoEtario")]
        public GrupoEtario GrupoEtario { get; set; }

        //FK Imagen
        [Column("codigo_imagen")]
        public int? CodigoImagen { get; set; }

        [ForeignKey("CodigoImagen")]
        public Imagen Imagen { get; set; }

        //FK Estado Mascota
        [Column("codigo_estado_mascota")]
        [Required]
        public int CodigoEstadoMascota { get; set; }

        [ForeignKey("CodigoEstadoMascota")]
        public EstadoMascota EstadoMascota { get; set; }

        //Relacion con Paseo Mascota
        public ICollection<PaseoMascota> PaseoMascotas { get; set; }

        //Relacion con Vacuna Mascota
        public ICollection<VacunaMascota> VacunaMascotas { get; set; }

        //Relacion con Compromiso
        public ICollection<Compromiso> Compromisos { get; set; }

        //Relacion con Recordatorio
        public ICollection<Recordatorio> Recordatorios { get; set; }

    }
}
