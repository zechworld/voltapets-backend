using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VoltaPetsAPI.Models
{
    [Table("region")]
    public class Region
    {
        [Column("codigo_region")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("Descripcion")]
        [Required]
        public string Descripcion { get; set; }

        //Relacion con Provincia
        public virtual ICollection<Provincia> Provincias { get; set; }

    }
}
