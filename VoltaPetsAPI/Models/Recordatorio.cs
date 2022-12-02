using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VoltaPetsAPI.Models
{
    [Table("recordatorio")]
    public class Recordatorio
    {
        [Column("codigo_recordatorio")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("titulo")]
        [Required]
        [StringLength(100)]
        public string Titulo { get; set; }

        [Column("descripcion")]
        [StringLength(500)]
        [Required]
        public string Descripcion { get; set; }

        [Column("fecha_publicacion")]
        [Required]
        public DateTime FechaPublicacion { get; set; }

        //FK Mascota
        [Column("codigo_mascota")]
        [Required]
        public int CodigoMascota { get; set; }

        [ForeignKey("CodigoMascota")]
        public Mascota Mascota { get; set; }

    }
}
