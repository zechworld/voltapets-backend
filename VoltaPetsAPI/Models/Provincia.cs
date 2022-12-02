using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VoltaPetsAPI.Models
{
    [Table("provincia")]
    public class Provincia
    {
        [Column("codigo_provincia")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("descripcion")]
        [Required]
        public string descripcion { get; set; }

        //FK Region
        [Column("codigo_region")]
        [Required]
        public int CodigoRegion { get; set; }

        [ForeignKey("CodigoRegion")]
        public Region Region { get; set; }

        //Relacion con Comuna
        public ICollection<Comuna> Comunas { get; set; }
    }
}
