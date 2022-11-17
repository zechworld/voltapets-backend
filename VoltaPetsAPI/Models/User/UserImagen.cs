using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VoltaPetsAPI.Models.User
{
    public class UserImagen
    {
        [Required]
        public int CodigoUsuario { get; set; }

        [Required]
        public string Public_Id { get; set; }

        [Required]
        [Url]
        public string Url { get; set; }

        [Required]
        public string Path { get; set; }
    }
}

