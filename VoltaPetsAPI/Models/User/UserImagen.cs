using System;
using System.ComponentModel.DataAnnotations;

namespace VoltaPetsAPI.Models.User
{
    public class UserImagen
    {
        [Required]
        public int CodigoUsuario { get; set; }

        [Required]
        [Url]
        public string Url { get; set; }

        [Required]
        public string Path { get; set; }
    }
}

