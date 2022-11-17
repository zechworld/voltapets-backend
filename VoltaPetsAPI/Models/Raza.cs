using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VoltaPetsAPI.Models
{
    [Table("raza")]
    public class Raza
    {
        [Column("codigo_raza")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("descripcion")]
        [Required]
        [StringLength(20)]
        public string Descripcion { get; set; }

        [Column("ppp")]
        [Required]
        public bool Ppp { get; set; }

        //Relacion con Mascota
        public virtual ICollection<Mascota> Mascotas { get; set; }

        //Relacion con Mascota Anuncio
        public virtual ICollection<MascotaAnuncio> MascotaAnuncios { get; set; }

    }
}
