using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VoltaPetsAPI.Models
{
    [Table("rango_tarifa")]
    public class RangoTarifa
    {
        [Column("codigo_rango")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("basico_inferior")]
        [Required]
        public int BasicoInferior { get; set; }

        [Column("basico_superior")]
        [Required]
        public int BasicoSuperior { get; set; }

        [Column("juego_inferior")]
        [Required]
        public int JuegoInferior { get; set; }

        [Column("juego_superior")]
        [Required]
        public int JuegoSuperior { get; set; }

        [Column("social_inferior")]
        [Required]
        public int SocialInferior { get; set; }

        [Column("social_superior")]
        [Required]
        public int SocialSuperior { get; set; }

        //FK Experiencia Paseador
        [Column("codigo_experiencia")]
        [Required]
        public int CodigoExperiencia { get; set; }

        [ForeignKey("CodigoExperiencia")]
        public ExperienciaPaseador ExperienciaPaseador { get; set; }

    }
}
