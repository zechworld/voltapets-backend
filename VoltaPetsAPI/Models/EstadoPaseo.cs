using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VoltaPetsAPI.Models
{
    [Table("estado_paseo")]
    public class EstadoPaseo
    {
        [Column("codigo_estado_paseo")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("descripcion")]
        [Required]
        [StringLength(15)]
        public string Descripcion { get; set; }

        //Relacion con Paseo
        public ICollection<Paseo> Paseos { get; set; }

    }
}
