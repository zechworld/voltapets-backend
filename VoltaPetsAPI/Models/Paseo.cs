using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VoltaPetsAPI.Models
{
    [Table("paseo")]
    public class Paseo
    {
        [Column("codigo_paseo")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CodigoPaseo { get; set; }

        [Column("fecha")]
        [Required]
        public DateTime Fecha { get; set; }

        [Column("hora_inicio")]
        [Required]
        public DateTime HoraInicio { get; set; }

        [Column("hora_termino")]
        [Required]
        public DateTime HoraTermino { get; set; }

        [Column("duracion_basico")]
        [Required]
        public TimeSpan DuracionBasico { get; set; }

        [Column("duracion_juego")]
        public TimeSpan? DuracionJuego { get; set; }

        [Column("duracion_social")]
        public TimeSpan? DuracionSocial { get; set; }

        [Column("calificado")]
        [Required]
        public bool Calificado { get; set; }

        //FK Paseador
        [Column("codigo_paseador")]
        [Required]
        public int CodigoPaseador { get; set; }

        [ForeignKey("CodigoPaseador")]
        public virtual Paseador Paseador { get; set; }

        //FK Estado Paseo
        [Column("codigo_estado_paseo")]
        [Required]
        public int CodigoEstadoPaseo { get; set; }

        [ForeignKey("CodigoEstadoPaseo")]
        public virtual EstadoPaseo EstadoPaseo { get; set; }

        //FK Parque
        [Column("codigo_parque")]
        public int? CodigoParque { get; set; }

        [ForeignKey("CodigoParque")]
        public virtual ParquePetFriendly ParquePetFriendly { get; set; }

        //FK Calificacion
        [Column("codigo_calificacion")]
        public int? CodigoCalificacion { get; set; }

        [ForeignKey("CodigoCalificacion")]
        public virtual Calificacion Calificacion { get; set; }

        //Relacion con Paseo Mascota
        public virtual ICollection<PaseoMascota> PaseoMascotas { get; set; }

    }
}
