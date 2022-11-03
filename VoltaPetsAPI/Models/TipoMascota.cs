using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VoltaPetsAPI.Models
{
    [Table("tipo_mascota")]
    public class TipoMascota
    {
        [Column("codigo_tipo_mascota")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CodigoTipoMascota { get; set; }

        [Column("descripcion")]
        [Required]
        [StringLength(10)]
        public string Descripcion { get; set; }

        //Relacion con Mascota Anuncio
        public virtual ICollection<MascotaAnuncio> MascotaAnuncios { get; set; }

    }
}
