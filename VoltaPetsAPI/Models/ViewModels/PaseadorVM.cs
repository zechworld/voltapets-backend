using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VoltaPetsAPI.Models.ViewModels
{
    public class PaseadorVM
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public string Apellido { get; set; }

        public string Descripcion { get; set; }

        public ExperienciaPaseador ExperienciaPaseador { get; set; }

        public Usuario Usuario { get; set; }

        public double? Calificacion { get; set; }

        public Tarifa TarifaActual { get; set; }

        public PerroAceptado PerroAceptado { get; set; }
    }
}
