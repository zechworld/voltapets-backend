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
        public int Id { get; set; }

        [Column("descripcion")]
        [Required]
        [StringLength(10)]
        public string Descripcion { get; set; }

        //Relacion con Mascota
        public ICollection<Mascota> Mascotas { get; set; }

        //Relacion con Mascota Anuncio
        public ICollection<MascotaAnuncio> MascotaAnuncios { get; set; }

    }
}
