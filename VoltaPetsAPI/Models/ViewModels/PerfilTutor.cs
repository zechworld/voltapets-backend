using System.ComponentModel.DataAnnotations;

namespace VoltaPetsAPI.Models.ViewModels
{
    public class PerfilTutor
    {
        //Datos Usuario
        [Required]
        public bool IsChangePassword { get; set; }

        public string Password { get; set; }

        [MinLength(6, ErrorMessage = "La contraseña debe tener como mínimo 6 carácteres")]
        [MaxLength(20, ErrorMessage = "La contraseña debe tener como máximo 20 carácteres")]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?\\d)(?=.*?[#?!@$%^&*-\\.,]).{6,}$", ErrorMessage = "Formato de contraseña incorrecto (Incluir Mayuscula, Minuscula, Numero, Caracter especial")]
        public string NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "Las contraseñas no coinciden")]
        public string ConfirmNewPassword { get; set; }

        //Datos Ubicacion
        [Required(ErrorMessage = "El campo Direccion es obligatorio")]
        [MaxLength(200, ErrorMessage = "La Direccion debe tener como maximo 200 caracteres")]
        public string Direccion { get; set; }

        public int? Departamento { get; set; }

        //FK Comuna
        [Required(ErrorMessage = "Se requiere codigo comuna")]
        public int CodigoComuna { get; set; }

        //Datos tutor
        [Required(ErrorMessage = "El campo Telefono es obligatorio")]
        [MaxLength(12, ErrorMessage = "El telefono debe tener como maximo 12 caracteres")]
        [RegularExpression("^(\\+56)(9)[98765432]\\d{7}$", ErrorMessage = "El formato de Telefono es incorrecto (Formato Ejemplo: +56964987115)")]
        public string Telefono { get; set; }

        [MaxLength(500, ErrorMessage = "La Descripcion debe tener como maximo 500 caracteres")]
        public string Descripcion { get; set; }
    }
}
