using System.ComponentModel.DataAnnotations;

namespace VoltaPetsAPI.Models.ViewModels
{
    public class UbicacionVM
    {
        //[Required(ErrorMessage = "El campo dirección es obligatorio")]
        //[MaxLength(200, ErrorMessage = "El campo dirección debe tener como máximo 200 carácteres")]
        //public string Direccion { get; set; }

        //FK Comuna
        [Required(ErrorMessage = "Se requiere que seleccione una comuna")]
        public int? CodigoComuna { get; set; }

    }
}
