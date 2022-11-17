using System;
using System.ComponentModel.DataAnnotations;

namespace VoltaPetsAPI.Models.ViewModels
{
    public class RecordatorioVM
    {
        [Required(ErrorMessage = "No se recibio codigo mascota")]
        public int? CodigoMascota { get; set; }

        [Required(ErrorMessage = "El campo Titulo es obligatorio")]
        [MaxLength(100, ErrorMessage = "El Titulo debe tener como maximo 100 caracteres")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "El campo Descripcion es obligatorio")]
        [MaxLength(500, ErrorMessage = "La Descripcion debe tener como maximo 500 caracteres")]
        public string Descripcion { get; set; }

    }
}
