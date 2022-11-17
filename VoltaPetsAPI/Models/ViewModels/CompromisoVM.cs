using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace VoltaPetsAPI.Models.ViewModels
{
    public class CompromisoVM
    {
        [Required(ErrorMessage = "El campo Titulo es obligatorio")]
        [MaxLength(100, ErrorMessage = "El campo Titulo debe tener como máximo 100 carácteres")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "El campo Fecha es obligatorio")]
        public DateTime? FechaCompromiso { get; set; }

        //FK Mascota
        [Required(ErrorMessage = "No se recibio codigo mascota")]
        public int? CodigoMascota { get; set; }

    }
}
