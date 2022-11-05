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
        public int CodigoUsuario { get; set; }

        [Column("email")]
        [Required(ErrorMessage ="El campo Correo es obligatorio")]
        [StringLength(200)]
        [MaxLength(200, ErrorMessage = "El Correo no puede exceder los 200 caracteres")]
        [EmailAddress(ErrorMessage ="El formato de Correo no es correcto")]
        public string Email { get; set; }

        [Column("password")]
        [Required(ErrorMessage ="El Campo Contraseña es obligatorio")]
        [StringLength(20)]
        [MinLength(6, ErrorMessage = "La contraseña debe tener como minimo 6 caracteres")]
        [MaxLength(20, ErrorMessage = "La contraseña debe tener como maximo 20 caracteres")]
        [RegularExpression("/^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?\\d)(?=.*?[#?!@$%^&*-\\.,]).{6,}$/g")]
        public string Password { get; set; }

        [Column("token")]
        public string? Token { get; set; }

        [NotMapped]
        [Compare("Password", ErrorMessage ="Las contraseñas no coinciden")]
        public string ConfirmPassword { get; set; }

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
