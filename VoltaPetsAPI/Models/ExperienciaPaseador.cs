using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VoltaPetsAPI.Models
{
    [Table("experiencia_paseador")]
    public class ExperienciaPaseador
    {
        [Column("codigo_experiencia")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("descripcion")]
        [Required]
        public string Descripcion { get; set; }

        //Relacion con Paseador
        public ICollection<Paseador> Paseadores { get; set; }

        //Relacion 1 a 1 con PerroPermitido
        public PerroPermitido PerroPermitido { get; set; }

        //Relacion 1 a 1 con RangoTarifa
        public RangoTarifa RangoTarifa { get; set; }

    }
}
