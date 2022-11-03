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
        public int CodigoExperiencia { get; set; }

        [Column("descripcion")]
        [Required]
        public string Descripcion { get; set; }

        //Relacion con Paseador
        public virtual ICollection<Paseador> Paseadores { get; set; }

        //Relacion 1 a 1 con PerroPermitido
        public virtual PerroPermitido PerroPermitido { get; set; }

        //Relacion 1 a 1 con RangoTarifa
        public virtual RangoTarifa RangoTarifa { get; set; }

    }
}
