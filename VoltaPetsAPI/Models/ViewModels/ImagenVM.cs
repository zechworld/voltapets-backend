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
    }
}

