using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VoltaPetsAPI.Models
{
    [Table("mascota_anuncio")]
    public class MascotaAnuncio
    {
        [Column("codigo_mascota_anuncio")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("nombre")]
        [Required]
        [StringLength(20)]
        public string Nombre { get; set; }

        [Column("edad")]
        public int? Edad { get; set; }

        //FK Tipo Mascota
        [Column("codigo_tipo_mascota")]
        [Required]
        public int CodigoTipoMascota { get; set; }

        [ForeignKey("CodigoTipoMascota")]
        public virtual TipoMascota TipoMascota { get; set; }

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

        //Relacion 1 a 1 con Anuncio
        public virtual Anuncio Anuncio { get; set; }

    }
}
