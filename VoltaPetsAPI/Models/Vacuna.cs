using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VoltaPetsAPI.Models
{
    [Table("vacuna")]
    public class Vacuna
    {
        [Column("codigo_vacuna")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("descripcion")]
        [Required]
        [StringLength(20)]
        public string Descripcion { get; set; }

        [Column("periodo")]
        [Required]
        public int Periodo { get; set; }

        [Column("unidad_medida")]
        [Required]
        [StringLength(20)]
        public string UnidadMedida { get; set; }

        [Column("obligatoria")]
        [Required]
        public bool Obligatoria { get; set; }

        //Relacion con Vacuna Mascota
        public virtual ICollection<VacunaMascota> VacunaMascotas { get; set; }

    }
}
