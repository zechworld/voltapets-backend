using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VoltaPetsAPI.Models
{
    [Table("puntaje_personalidad")]
    public class PuntajePersonalidad
    {
        [Column("codigo_puntaje")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("amigable")]
        [Required]
        public float Amigable { get; set; }

        [Column("jugueton")]
        [Required]
        public float Jugueton { get; set; }

        [Column("sociable")]
        [Required]
        public float Sociable { get; set; }

        [Column("nervioso")]
        [Required]
        public float Nervioso { get; set; }

        [Column("timido")]
        [Required]
        public float Timido { get; set; }

        [Column("miedoso")]
        [Required]
        public float Miedoso { get; set; }

        [Column("dominante")]
        [Required]
        public float Dominante { get; set; }

        [Column("territorial")]
        [Required]
        public float Territorial { get; set; }

        [Column("agresivo")]
        [Required]
        public float Agresivo { get; set; }

        //FK Paseo Mascota
        [Column("codigo_paseo_mascota")]
        [Required]
        public int CodigoPaseoMascota { get; set; }

        [ForeignKey("CodigoPaseoMascota")]
        public PaseoMascota PaseoMascota { get; set; }

    }
}
