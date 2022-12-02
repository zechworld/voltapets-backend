using System;
using System.ComponentModel.DataAnnotations;

namespace VoltaPetsAPI.Models.ViewModels
{
    public class ImagenVM
    {
        [Required]
        [Url]
        public string Url { get; set; }

        [Required]
        public string Path { get; set; }

        [Required]
        public string Public_Id { get; set; }

        public Imagen ToImagen()
        {
            Imagen imagen = new Imagen
            {
                Public_Id = Public_Id,
                Url = Url,
                Path = Path,
            };

            return imagen;
        }

    }
}

