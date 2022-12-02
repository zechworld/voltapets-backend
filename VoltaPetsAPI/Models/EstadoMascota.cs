using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VoltaPetsAPI.Models
{
    [Table("estado_mascota")]
    public class EstadoMascota
    {
        [Column("codigo_estado_mascota")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("descripcion")]
        [Required]
        [StringLength(20)]
        public string Descripcion { get; set; }

        //Relacion con Mascota
        public ICollection<Mascota> Mascotas { get; set; }

    }
}
