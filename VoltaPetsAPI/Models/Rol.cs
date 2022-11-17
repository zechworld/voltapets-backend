using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VoltaPetsAPI.Models
{
    [Table("rol")]
    public class Rol
    {
        [Column("codigo_rol")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("descripcion")]
        [Required]
        [StringLength(20)]
        public string Descripcion { get; set; }

        //Relacion con Usuario
        public virtual ICollection<Usuario> Usuarios { get; set; }

    }
}
