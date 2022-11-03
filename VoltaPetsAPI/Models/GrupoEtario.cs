using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VoltaPetsAPI.Models
{
    [Table("grupo_etario")]
    public class GrupoEtario
    {
        [Column("codigo_etario")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CodigoEtario { get; set; }

        [Column("descripcion")]
        [Required]
        [StringLength(20)]
        public string Descripcion { get; set; }

        [Column("edad_inferior")]
        [Required]
        [StringLength(20)]
        public string EdadInferior { get; set; }

        [Column("edad_superior")]
        [Required]
        [StringLength(20)]
        public string EdadSuperior { get; set; }

        //Relacion con Mascota
        public virtual ICollection<Mascota> Mascotas { get; set; }

        //Relacion con Mascota Anuncio
        public virtual ICollection<MascotaAnuncio> MascotaAnuncios { get; set; }

    }
}
