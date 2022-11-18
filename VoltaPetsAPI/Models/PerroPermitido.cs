using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VoltaPetsAPI.Models
{
    [Table("perro_permitido")]
    public class PerroPermitido
    {
        [Column("codigo_permitido")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("tamanio_mediano")]
        [Required]
        public bool TamanioMediano { get; set; }

        [Column("tamanio_grande")]
        [Required]
        public bool TamanioGrande { get; set; }

        [Column("tamanio_gigante")]
        [Required]
        public bool TamanioGigante { get; set; }

        //FK Experiencia Paseador
        [Column("codigo_experiencia")]
        [Required]
        public int CodigoExperiencia { get; set; }

        [ForeignKey("CodigoExperiencia")]
        public ExperienciaPaseador ExperienciaPaseador { get; set; }

    }
}
