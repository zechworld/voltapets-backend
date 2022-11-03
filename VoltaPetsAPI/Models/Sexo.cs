using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VoltaPetsAPI.Models
{
    [Table("sexo")]
    public class Sexo
    {
        [Column("codigo_sexo")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CodigoSexo { get; set; }

        [Column("descripcion")]
        [Required]
        [StringLength(10)]
        public string Descripcion { get; set; }

        //Relacion con Mascota
        public virtual ICollection<Mascota> Mascotas { get; set; }

        //Relacion con Mascota Anuncio
        public virtual ICollection<MascotaAnuncio> MascotaAnuncios { get; set; }

    }
}
