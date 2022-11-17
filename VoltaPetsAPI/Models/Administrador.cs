using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VoltaPetsAPI.Models
{
    [Table("administrador")]
    public class Administrador
    {
        [Column("codigo_administrador")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("nombre")]
        [Required]
        [StringLength(20)]
        public string Nombre { get; set; }

        [Column("apellido")]
        [Required]
        [StringLength(20)]
        public string Apellido { get; set; }

        //FK Usuario
        [Column("codigo_usuario")]
        [Required]
        public int CodigoUsuario { get; set; }

        [ForeignKey("CodigoUsuario")]
        public virtual Usuario Usuario { get; set; }

    }
}
