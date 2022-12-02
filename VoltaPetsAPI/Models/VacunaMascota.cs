using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VoltaPetsAPI.Models
{
    [Table("vacuna_mascota")]
    public class VacunaMascota
    {
        [Column("codigo_vacuna_mascota")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("fecha_vacunacion")]
        [Required]
        public DateTime FechaVacunacion { get; set; }

        //FK Vacuna
        [Column("codigo_vacuna")]
        [Required]
        public int CodigoVacuna { get; set; }

        [ForeignKey("CodigoVacuna")]
        public Vacuna Vacuna { get; set; }

        //FK Mascota
        [Column("codigo_mascota")]
        [Required]
        public int CodigoMascota { get; set; }

        [ForeignKey("CodigoMascota")]
        public Mascota Mascota { get; set; }

        //FK Imagen
        [Column("codigo_imagen")]
        public int? CodigoImagen { get; set; }

        [ForeignKey("CodigoImagen")]
        public Imagen Imagen { get; set; }

    }
}
