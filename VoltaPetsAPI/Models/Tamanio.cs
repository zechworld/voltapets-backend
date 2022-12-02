using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VoltaPetsAPI.Models
{
    [Table("tamanio")]
    public class Tamanio
    {
        [Column("codigo_tamanio")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("descripcion")]
        [Required]
        [StringLength(20)]
        public string Descripcion { get; set; }

        [Column("peso_inferior")]
        [Required]
        public double PesoInferior { get; set; }

        [Column("peso_superior")]
        [Required]
        public double PesoSuperior { get; set; }

        [Column("altura_inferior")]
        [Required]
        public double AlturaInferior { get; set; }

        [Column("altura_superior")]
        [Required]
        public double AlturaSuperior { get; set; }

        //Relacion con Mascota
        public ICollection<Mascota> Mascotas { get; set; }

        //Relacion con Mascota Anuncio
        public ICollection<MascotaAnuncio> MascotaAnuncios { get; set; }

    }
}
