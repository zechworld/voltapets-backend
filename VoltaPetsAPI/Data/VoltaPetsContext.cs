using Microsoft.EntityFrameworkCore;
using VoltaPetsAPI.Models;

namespace VoltaPetsAPI.Data
{
    public class VoltaPetsContext : DbContext
    {
        public VoltaPetsContext(DbContextOptions<VoltaPetsContext> options) : base(options) 
        { 

        }

        public DbSet<Administrador> Administradores { get; set; }
        public DbSet<Anuncio> Anuncios { get; set; }
        public DbSet<Calificacion> Calificaciones { get; set; }
        public DbSet<Comportamiento> Comportamientos { get; set; }
        public DbSet<Compromiso> Compromisos { get; set; }
        public DbSet<Comuna> Comunas { get; set; }
        public DbSet<EstadoMascota> EstadoMascotas { get; set; }
        public DbSet<EstadoPaseo> EstadoPaseos { get; set; }
        public DbSet<ExperienciaPaseador> ExperienciaPaseadores { get; set; }
        public DbSet<GrupoEtario> GrupoEtarios { get; set; }
        public DbSet<Imagen> Imagenes { get; set; }
        public DbSet<Mascota> Mascotas { get; set; }
        public DbSet<MascotaAnuncio> MascotaAnuncios { get; set; }
        public DbSet<ParquePetFriendly> ParquePetFriendlies { get; set; }
        public DbSet<Paseador> Paseadores { get; set; }
        public DbSet<Paseo> Paseos { get; set; }
        public DbSet<PaseoMascota> PaseoMascotas { get; set; }
        public DbSet<PerroAceptado> PerroAceptados { get; set; }
        public DbSet<PerroPermitido> PerroPermitidos { get; set; }
        public DbSet<Ponderacion> Ponderaciones { get; set; }
        public DbSet<Provincia> Provincias { get; set; }
        public DbSet<PuntajePersonalidad> PuntajePersonalidades { get; set; }
        public DbSet<RangoTarifa> RangoTarifas { get; set; }
        public DbSet<Raza> Razas { get; set; }
        public DbSet<Recordatorio> Recordatorios { get; set; }
        public DbSet<Region> Regiones { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Sexo> Sexos { get; set; }
        public DbSet<Tamanio> Tamanios { get; set; }
        public DbSet<Tarifa> Tarifas { get; set; }
        public DbSet<TipoAnuncio> TipoAnuncios { get; set; }
        public DbSet<TipoMascota> TipoMascotas { get; set; }
        public DbSet<Tutor> Tutores { get; set; }
        public DbSet<Ubicacion> Ubicaciones { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Vacuna> Vacunas { get; set; }
        public DbSet<VacunaMascota> VacunaMascotas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //ExperienciaPaseador relacion 1 a 1 PerroPermitido
            modelBuilder.Entity<ExperienciaPaseador>().HasOne(ep => ep.PerroPermitido).WithOne(pp => pp.ExperienciaPaseador);

            //ExperienciaPaseador relacion 1 a 1 RangoTarifa
            modelBuilder.Entity<ExperienciaPaseador>().HasOne(ep => ep.RangoTarifa).WithOne(rt => rt.ExperienciaPaseador);

            //Paseador relacion 1 a 1 PerroAceptado
            modelBuilder.Entity<Paseador>().HasOne(p => p.PerroAceptado).WithOne(pa => pa.Paseador);

            //PaseoMascota relacion 1 a 1 PuntajePersonalidad
            modelBuilder.Entity<PaseoMascota>().HasOne(pm => pm.PuntajePersonalidad).WithOne(ptj => ptj.PaseoMascota);

            //PaseoMascota relacion 1 a 1 Comportamiento
            modelBuilder.Entity<PaseoMascota>().HasOne(pm => pm.Comportamiento).WithOne(c => c.PaseoMascota);

            //Usuario relacion 1 a 1 con Administrador
            modelBuilder.Entity<Usuario>().HasOne(u => u.Administrador).WithOne(a => a.Usuario);

            //Usuario relacion 1 a 1 con Paseador
            modelBuilder.Entity<Usuario>().HasOne(u => u.Paseador).WithOne(p => p.Usuario);

            //Usuario relacion 1 a 1 con Tutor
            modelBuilder.Entity<Usuario>().HasOne(u => u.Tutor).WithOne(t => t.Usuario);

            //Imagen relacion 1 a 1 con Usuario
            modelBuilder.Entity<Imagen>().HasOne(img => img.Usuario).WithOne(u => u.Imagen);

            //Imagen relacion 1 a 1 con Mascota
            modelBuilder.Entity<Imagen>().HasOne(img => img.Mascota).WithOne(m => m.Imagen);

            //Imagen relacion 1 a 1 con VacunaMascota
            modelBuilder.Entity<Imagen>().HasOne(img => img.VacunaMascota).WithOne(vm => vm.Imagen);

            //Imagen relacion 1 a 1 con Anuncio
            modelBuilder.Entity<Imagen>().HasOne(img => img.Anuncio).WithOne(a => a.Imagen);

            //MascotaAnuncio relacion 1 a 1 con Anuncio
            modelBuilder.Entity<MascotaAnuncio>().HasOne(ma => ma.Anuncio).WithOne(a => a.MascotaAnuncio);

        }

    }
}
