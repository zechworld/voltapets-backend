using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VoltaPetsAPI.Models
{
    [Table("imagen")]
    public class Imagen
    {
        [Column("codigo_imagen")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CodigoImagen { get; set; }

        [Column("path")]
        [Required]
        public string Path { get; set; }

        [Column("url")]
        [Required]
        [Url]
        public string Url { get; set; }

        //Relacion 1 a 1 con Usuario
        public virtual Usuario Usuario { get; set; }

        //Relacion 1 a 1 con Mascota
        public virtual Mascota Mascota { get; set; }

        //Relacion 1 a 1 con VacunaMascota
        public virtual VacunaMascota VacunaMascota { get; set; }

        //Relacion 1 a 1 con Anuncio
        public virtual Anuncio Anuncio { get; set; }

    }
}
