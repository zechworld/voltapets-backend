using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace VoltaPetsAPI.Models.ViewModels
{
    public class ParametroLaboralVM
    {
        //datos tarifa
        [Required(ErrorMessage = "No se recibio una tarfia de paseo de necesidades básicas de mascotas")]
        public int? Basico { get; set; }

        [Required(ErrorMessage = "No se recibio una tarfia de juego con las mascotas")]
        public int? Juego { get; set; }

        [Required(ErrorMessage = "No se recibio una tarifa de socialización con otras de mascotas")]
        public int? Social { get; set; }

        //datos perro aceptado
        [Required(ErrorMessage = "No se recibio una cantidad de mascotas")]
        [Range(0,4, ErrorMessage = "La Cantidad de mascotas se encuentra en fuera del rango entre {1} y {2}")]
        public int? CantidadPerro { get; set; }

        public bool TamanioToy { get; set; }

        public bool TamanioPequenio { get; set; }

        public bool TamanioMediano { get; set; }

        public bool TamanioGrande { get; set; }

        public bool TamanioGigante { get; set; }
    }
}
