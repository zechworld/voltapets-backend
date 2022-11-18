using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VoltaPetsAPI.Models
{
    [Table("tipo_anuncio")]
    public class TipoAnuncio
    {
        [Column("codigo_tipo_anuncio")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("descripcion")]
        [Required]
        [StringLength(10)]
        public string Descripcion { get; set; }

        //Relacion con Anuncio
        public ICollection<Anuncio> Anuncios { get; set; }
        
    }
}
