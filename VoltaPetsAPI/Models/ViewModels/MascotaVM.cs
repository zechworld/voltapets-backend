using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using VoltaPetsAPI.Helpers;

namespace VoltaPetsAPI.Models.ViewModels
{
    public class MascotaVM
    {        
        [Required]
        [MinLength(2, ErrorMessage = "El Nombre debe tener como mínimo 2 carácteres")]
        [MaxLength(20, ErrorMessage = "El Nombre debe tener como máximo 20 carácteres")]        
        [RegularExpression("^([a-zA-ZÀ-ÿ\\u00f1\\u00d1]+)$", ErrorMessage = "Formato de nombre incorrecto (El Nombre no debe contener números o carácteres especiales)")]
        public string Nombre { get; set; }

        [MaxLength(500, ErrorMessage = "La Descripcion debe tener como máximo 500 carácteres")]
        public string? Descripcion { get; set; }

        [Required]
        [Fecha(ErrorMessage = "Formato incorrecto de Fecha (ISO 8601)")]
        public DateTime FechaNacimiento { get; set; }

        public bool IsFechaNacimiento { get; set; }

        public bool Esterilizado { get; set; }

        [Range(0,29, ErrorMessage = "La Edad debe encontrarse en un rango entre {1} y {2} años")]
        public double? EdadRegistro { get; set; }

        public bool IsYear { get; set; }

        //FK Sexo
        [Required]
        public int? CodigoSexo { get; set; }

        //FK Tamanio
        [Required]
        public int? CodigoTamanio { get; set; }

        //FK Raza
        [Required]
        public int? CodigoRaza { get; set; }     

    }
}
