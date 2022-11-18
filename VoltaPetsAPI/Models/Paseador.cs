using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VoltaPetsAPI.Models
{
    [Table("paseador")]
    public class Paseador
    {
        [Column("codigo_paseador")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("rut")]
        [Required]
        [StringLength(11)]
        public string Rut { get; set; }

        [Column("dv")]
        [Required]
        [StringLength(1)]
        public string Dv { get; set; }

        [Column("nombre")]
        [Required]
        [StringLength(40)]
        public string Nombre { get; set; }

        [Column("apellido")]
        [Required]
        [StringLength(40)]
        public string Apellido { get; set; }

        [Column("telefono")]
        [Required]
        [StringLength(12)]
        public string Telefono { get; set; }

        [Column("descripcion")]
        [StringLength(500)]
        public string? Descripcion { get; set; }

        [Column("activado")]
        [Required]
        public bool Activado { get; set; }

        //FK Usuario
        [Column("codigo_usuario")]
        [Required]
        public int CodigoUsuario { get; set; }

        [ForeignKey("CodigoUsuario")]
        public Usuario Usuario { get; set; }

        //FK Ubicacion
        [Column("codigo_ubicacion")]
        [Required]
        public int CodigoUbicacion { get; set; }

        [ForeignKey("CodigoUbicacion")]
        public Ubicacion Ubicacion { get; set; }

        //FK Experiencia Paseador
        [Column("codigo_experiencia")]
        public int CodigoExperiencia { get; set; }

        [ForeignKey("CodigoExperiencia")]
        public ExperienciaPaseador ExperienciaPaseador { get; set; }

        //Relacion con Paseo
        public ICollection<Paseo> Paseos { get; set; }

        //Relacion con Tarifa
        public ICollection<Tarifa> Tarifas { get; set; }

        //Relacion 1 a 1 con PerroAceptado
        public PerroAceptado PerroAceptado { get; set; }

    }
}
