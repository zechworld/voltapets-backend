using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VoltaPetsAPI.Models
{
    [Table("usuario")]
    public class Usuario
    {
        [Column("codigo_usuario")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("email")]
        [Required]
        [StringLength(200)]
        public string Email { get; set; }

        [Column("password")]
        [Required]
        [StringLength(70)]
        public string Password { get; set; }

        [Column("token")]
        public string? Token { get; set; }

        //FK Rol
        [Column("codigo_rol")]
        [Required]
        public int CodigoRol { get; set; }

        [ForeignKey("CodigoRol")]
        public virtual Rol Rol { get; set; }

        //FK Imagen
        [Column("codigo_imagen")]
        public int? CodigoImagen { get; set; }

        [ForeignKey("CodigoImagen")]
        public virtual Imagen Imagen { get; set; }

        //Relacion 1 a 1 con Administrador
        public virtual Administrador Administrador { get; set; }

        //Relacion 1 a 1 con Paseador
        public virtual Paseador Paseador { get; set; }

        //Relacion 1 a 1 con Tutor
        public virtual Tutor Tutor { get; set; }

    }
}
