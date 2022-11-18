using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VoltaPetsAPI.Models
{
    [Table("calificacion")]
    public class Calificacion
    {
        [Column("codigo_calificacion")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("valor")]
        [Required]
        public float Valor { get; set; }

        //Relacion con Paseo
        public ICollection<Paseo> Paseos { get; set; }
            
    }
}
