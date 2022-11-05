using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VoltaPetsAPI.Models.User
{
    public class UserTutor
    {
        //Datos Usuario
        [Required(ErrorMessage = "El campo Correo es obligatorio")]
        [MaxLength(200, ErrorMessage = "El Correo no puede exceder los 200 caracteres")]
        [EmailAddress(ErrorMessage = "El formato de Correo no es correcto")]
        public string Email { get; set; }

        [Required(ErrorMessage = "El Campo Contraseña es obligatorio")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener como minimo 6 caracteres")]
        [MaxLength(20, ErrorMessage = "La contraseña debe tener como maximo 20 caracteres")]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?\\d)(?=.*?[#?!@$%^&*-\\.,]).{6,}$")]// agregar mensaje
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
        public string ConfirmPassword { get; set; }

        //FK Rol
        /*
        [Required(ErrorMessage = "Se requiere codigo rol")]
        public int CodigoRol { get; set; }
        */

        //Datos Ubicacion
        [Required(ErrorMessage = "El campo Direccion es obligatorio")]
        [MaxLength(200, ErrorMessage = "La Direccion debe tener como maximo 200 caracteres")]
        public string Direccion { get; set; }

        public int? Departamento { get; set; }

        /*
        public double? Latitud { get; set; }

        public double? Longitud { get; set; }
        */

        //FK Comuna
        [Required(ErrorMessage = "Se requiere codigo comuna")]
        public int CodigoComuna { get; set; }

        //Datos Tutor
        [Required(ErrorMessage = "El campo Nombre es obligatorio")]
        [MinLength(2, ErrorMessage = "El Nombre debe tener como minimo 2 caracteres")]
        [MaxLength(40, ErrorMessage = "El Nombre debe tener como maximo 40 caracteres")]
        [RegularExpression("^([a-zA-ZÀ-ÿ\\u00f1\\u00d1]+\\s?([a-zA-ZÀ-ÿ\\u00f1\\u00d1]*)*[a-zA-ZÀ-ÿ\\u00f1\\u00d1]+)$", ErrorMessage = "Formato de Nombre incorrecto (Ingrese maximo 2 nombres sin numeros ni caracteres especiales)")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo Apellido es obligatorio")]
        [MinLength(2, ErrorMessage = "El Apellido debe tener como minimo 2 caracteres")]
        [MaxLength(40, ErrorMessage = "El Apellido debe tener como maximo 40 caracteres")]
        [RegularExpression("^([a-zA-ZÀ-ÿ\\u00f1\\u00d1]+\\s?([a-zA-ZÀ-ÿ\\u00f1\\u00d1]*)*[a-zA-ZÀ-ÿ\\u00f1\\u00d1]+)$", ErrorMessage = "Formato de Apellido incorrecto (Ingrese maximo 2 Apellidos sin numeros ni caracteres especiales)")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "El campo Telefono es obligatorio")]
        [MinLength(12, ErrorMessage = "El telefono debe tener como minimo 12 caracteres")]
        [MaxLength(12, ErrorMessage = "El telefono debe tener como maximo 12 caracteres")]
        [RegularExpression("^(\\+56)(9)[98765432]\\d{7}$", ErrorMessage = "El formato de Telefono es incorrecto (Formato Ejemplo: +56964987115)")]
        public string Telefono { get; set; }

        [MaxLength(500, ErrorMessage = "La Descripcion tiene como maximo 500 caracteres")]
        public string? Descripcion { get; set; }

        /*
        public bool? Activado { get; set; }
        */

        //FK Usuario
        public int CodigoUsuario { get; set; }

        //FK Ubicacion
        public int CodigoUbicacion { get; set; }

    }
}
