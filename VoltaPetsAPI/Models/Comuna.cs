using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VoltaPetsAPI.Models
{
    [Table("comuna")]
    public class Comuna
    {
        [Column("codigo_comuna")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("descripcion")]
        [Required]
        public string Descripcion { get; set; }

        //FK Provincia
        [Column("codigo_provincia")]
        [Required]
        public int CodigoProvincia { get; set; }

        [ForeignKey("CodigoProvincia")]
        public virtual Provincia Provincia { get; set; }

        //Relacion con Ubicacion
        public virtual ICollection<Ubicacion> Ubicaciones { get; set; }

        //Relacion con Anuncio
        public virtual ICollection<Anuncio> Anuncios { get; set; }

    }
}
