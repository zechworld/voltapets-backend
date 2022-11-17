using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VoltaPetsAPI.Models
{
    [Table("anuncio")]
    public class Anuncio
    {
        [Column("codigo_anuncio")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("titulo")]
        [StringLength(100)]
        public string? Titulo { get; set; }

        [Column("descripcion")]
        [Required]
        [StringLength(500)]
        public string Descripcion { get; set; }

        [Column("telefono")]
        [Required]
        [StringLength(15)]
        public string Telefono { get; set; }

        [Column("recompensa")]
        public int? Recompensa { get; set; }

        [Column("responsable")]
        [Required]
        [StringLength(50)]
        public string Responsable { get; set; }

        //FK Tutor
        [Column("codigo_tutor")]
        [Required]
        public int CodigoTutor { get; set; }

        [ForeignKey("CodigoTutor")]
        public virtual Tutor Tutor { get; set; }

        //FK Tipo Anuncio
        [Column("codigo_tipo_anuncio")]
        [Required]
        public int CodigoTipoAnuncio { get; set; }

        [ForeignKey("CodigoTipoAnuncio")]
        public virtual TipoAnuncio TipoAnuncio { get; set; }

        //FK Mascota Anuncio
        [Column("codigo_mascota_anuncio")]
        [Required]
        public int CodigoMascotaAnuncio { get; set; }

        [ForeignKey("CodigoMascotaAnuncio")]
        public virtual MascotaAnuncio MascotaAnuncio { get; set; }

        //FK Imagen
        [Column("codigo_imagen")]
        [Required]
        public int CodigoImagen { get; set; }

        [ForeignKey("CodigoImagen")]
        public virtual Imagen Imagen { get; set; }

        //FK Comuna
        [Column("codigo_comuna")]
        [Required]
        public int CodigoComuna { get; set; }

        [ForeignKey("CodigoComuna")]
        public virtual Comuna Comuna { get; set; }

    }
}
