using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VoltaPetsAPI.Models
{
    [Table("tutor")]
    public class Tutor
    {
        [Column("codigo_tutor")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CodigoTutor { get; set; }

        [Column("nombre")]
        [Required]
        [StringLength(40)]
        public string Nombre { get; set; }

        [Column("apellido")]
        [Required]
        [StringLength(40)]
        public string Apellido { get; set; }

        [Column("telefono")]
        [Required]
        [StringLength(12)]
        public string Telefono { get; set; }

        [Column("descripcion")]
        [StringLength(500)]
        public string? Descripcion { get; set; }

        [Column("activado")]
        [Required]
        public bool Activado { get; set; }

        //FK Usuario
        [Column("codigo_usuario")]
        [Required]
        public int CodigoUsuario { get; set; }

        [ForeignKey("CodigoUsuario")]
        public virtual Usuario Usuario { get; set; }

        //FK Ubicacion
        [Column("codigo_ubicacion")]
        [Required]
        public int CodigoUbicacion { get; set; }

        [ForeignKey("CodigoUbicacion")]
        public virtual Ubicacion Ubicacion { get; set; }

        //Relacion con Mascota
        public virtual ICollection<Mascota> Mascotas { get; set; }

        //Relacion con Anuncio
        public virtual ICollection<Anuncio> Anuncios { get; set; }

    }
}
