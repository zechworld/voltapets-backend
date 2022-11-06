using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VoltaPetsAPI.Models.User
{
    public class UserLogin
    {
        [Column("email")]
        [Required(ErrorMessage = "El campo Correo es obligatorio")]
        [StringLength(200)]
        [MaxLength(200, ErrorMessage = "El Correo no puede exceder los 200 caracteres")]
        [EmailAddress(ErrorMessage = "El formato de Correo no es correcto")]
        public string Email { get; set; }

        [Column("password")]
        [Required(ErrorMessage = "El Campo Contraseña es obligatorio")]
        [StringLength(70)]
        [MinLength(6, ErrorMessage = "La contraseña debe tener como minimo 6 caracteres")]
        [MaxLength(20, ErrorMessage = "La contraseña debe tener como maximo 20 caracteres")]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?\\d)(?=.*?[#?!@$%^&*-\\.,]).{6,}$", ErrorMessage = "Formato de contraseña incorrecto (Incluir Mayuscula, Minuscula, Numero, Caracter especial")]
        public string Password { get; set; }
    }
}
