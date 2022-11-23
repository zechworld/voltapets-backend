using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace VoltaPetsAPI.Models.ViewModels
{
    public class VacunaMascotaVM
    {
        [Required(ErrorMessage = "No se recibió nombre vacuna")]
        public string NombreVacuna { get; set; }

        public DateTime? FechaVacunacion { get; set; }

        public bool Obligatoria { get; set; }

        //FK Vacuna
        [Required(ErrorMessage = "No se recibió codigo vacuna")]
        public int? CodigoVacuna { get; set; }

        //FK Mascota
        [Required(ErrorMessage = "No se recibió codigo mascota")]
        public int? CodigoMascota { get; set; }

        public Imagen Imagen { get; set; }
    }
}
