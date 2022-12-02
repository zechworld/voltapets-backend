using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VoltaPetsAPI.Models
{
    [Table("perro_aceptado")]
    public class PerroAceptado
    {
        [Column("codigo_aceptado")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("cantidad_perro")]
        [Required]
        public int CantidadPerro { get; set; }

        [Column("tamanio_toy")]
        [Required]
        public bool TamanioToy { get; set; }

        [Column("tamanio_pequenio")]
        [Required]
        public bool TamanioPequenio { get; set; }

        [Column("tamanio_mediano")]
        [Required]
        public bool TamanioMediano { get; set; }

        [Column("tamanio_grande")]
        [Required]
        public bool TamanioGrande { get; set; }

        [Column("tamanio_gigante")]
        [Required]
        public bool TamanioGigante { get; set; }

        //FK Paseador
        [Column("codigo_paseador")]
        [Required]
        public int CodigoPaseador { get; set; }

        [ForeignKey("CodigoPaseador")]
        public Paseador Paseador { get; set; }

    }
}
