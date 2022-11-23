using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using VoltaPetsAPI.Data;
using VoltaPetsAPI.Models.ViewModels;
using VoltaPetsAPI.Models;
using CloudinaryDotNet;
using VoltaPetsAPI.Helpers;

namespace VoltaPetsAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class VacunaMascotaController : ControllerBase
    {
        private readonly VoltaPetsContext _context;
        private readonly Cloudinary _cloudinary;

        public VacunaMascotaController(VoltaPetsContext context, Cloudinary cloudinary)
        {
            _context = context;
            _cloudinary = cloudinary;
        }

        [HttpGet]
        [Route("Obtener/{codigoMascota:int}")]
        [Authorize(Policy = "Tutor")]
        public async Task<IActionResult> ObtenerVacunasMascota(int codigoMascota)
        {
            var vacunas = await _context.Vacunas
                    .Select(v => new Vacuna
                    {
                        Id = v.Id,
                        Descripcion = v.Descripcion,
                        Obligatoria = v.Obligatoria
                    }).AsNoTracking()
                    .ToListAsync();

            if (!vacunas.Any())
            {
                return NotFound(new { mensaje = "No existen vacunas en la base de datos" });
            }

            var vacunasMascota = await _context.VacunaMascotas
                .Include(vm => vm.Vacuna)
                .Include(vm => vm.Imagen)
                .Where(vm => vm.CodigoMascota == codigoMascota)
                .Select(vm => new VacunaMascota
                {
                    FechaVacunacion = vm.FechaVacunacion,
                    CodigoVacuna = vm.CodigoVacuna,
                    CodigoMascota = vm.CodigoMascota,
                    Vacuna = new Vacuna
                    {
                        Id = vm.Vacuna.Id,
                        Descripcion = vm.Vacuna.Descripcion,
                        Obligatoria = vm.Vacuna.Obligatoria
                    },
                    Imagen = vm.Imagen                    
                }).AsNoTracking()
                .ToListAsync();

            List<VacunaMascotaVM> vacunasMascotaVM = new List<VacunaMascotaVM>();

            if (!vacunasMascota.Any())
            {
                foreach (var v in vacunas)
                {
                    VacunaMascotaVM vacunaVM = new VacunaMascotaVM
                    {
                        CodigoMascota = codigoMascota,
                        CodigoVacuna = v.Id,
                        NombreVacuna = v.Descripcion,
                        FechaVacunacion = null,
                        Obligatoria = v.Obligatoria,
                        Imagen = null
                    };

                    vacunasMascotaVM.Add(vacunaVM);
                }
            }
            else
            {
                foreach (var v in vacunas)
                {
                    VacunaMascotaVM vacunaVM;

                    VacunaMascota vacuna = vacunasMascota.FirstOrDefault(vm => vm.CodigoVacuna == v.Id);
                    
                    if (vacuna != null)
                    {
                        vacunaVM = new VacunaMascotaVM
                        {
                            CodigoMascota = vacuna.CodigoMascota,
                            CodigoVacuna = vacuna.CodigoVacuna,
                            NombreVacuna = vacuna.Vacuna.Descripcion,
                            FechaVacunacion = vacuna.FechaVacunacion,
                            Obligatoria = vacuna.Vacuna.Obligatoria,
                            Imagen = vacuna.Imagen
                        };

                        vacunasMascotaVM.Add(vacunaVM);
                    }
                    else
                    {
                        vacunaVM = new VacunaMascotaVM
                        {
                            CodigoMascota = codigoMascota,
                            CodigoVacuna = v.Id,
                            NombreVacuna = v.Descripcion,
                            FechaVacunacion = null,
                            Obligatoria = v.Obligatoria,
                            Imagen = null
                        };

                        vacunasMascotaVM.Add(vacunaVM);
                    }
                }
            }                                           

            return Ok(vacunasMascotaVM);

        }

        [HttpPost]
        [Route("Registrar")]
        [Authorize(Policy = "Tutor")]
        public async Task<IActionResult> RegistrarVacunaMascota(List<VacunaMascotaVM> vacunasVM)
        {
            //validar que no se recibe una lista vacia
            if (!vacunasVM.Any())
            {
                return BadRequest(new { mensaje = "No se recibieron vacunas para registrar" });
            }

            //remover vacunas que no fueron registradas
            foreach (var vacunaVM in vacunasVM)
            {
                if (vacunaVM.FechaVacunacion == null)
                {
                    vacunasVM.Remove(vacunaVM);
                }
            }

            if (!ModelState.IsValid)
            {
                foreach (var vacVM in vacunasVM)
                {
                    ImagenCloudinary.EliminarImagenHosting(_cloudinary, vacVM.Imagen);
                }
                return BadRequest(ModelState);
            }

            int? codigoMascota = vacunasVM.First().CodigoMascota;

            if (codigoMascota == null)
            {
                foreach (var vacVM in vacunasVM)
                {
                    ImagenCloudinary.EliminarImagenHosting(_cloudinary, vacVM.Imagen);
                }
                return BadRequest(new { mensaje = "No se recibio el codigo de la mascota" });
            }

            if (!vacunasVM.TrueForAll(v => v.CodigoMascota == codigoMascota))
            {
                foreach (var vacVM in vacunasVM)
                {
                    ImagenCloudinary.EliminarImagenHosting(_cloudinary, vacVM.Imagen);
                }
                return BadRequest(new { mensaje = "Error: Existe una vacuna con un codigo de mascota diferente" });
            }

            var mascota = await _context.Mascotas.Include(m => m.VacunaMascotas).FirstOrDefaultAsync(m => m.Id == (int)codigoMascota);

            if (mascota == null)
            {
                foreach (var vacVM in vacunasVM)
                {
                    ImagenCloudinary.EliminarImagenHosting(_cloudinary, vacVM.Imagen);
                }
                return BadRequest(new { mensaje = "No se pudo obtener la mascota" });
            }

            //validar fecha minima de vacunacion
            double edadMascota = 0.0;

            if (mascota.EdadRegistro == null)
            {
                edadMascota = CalcularAnios(mascota.FechaNacimiento);
            }
            else
            {
                edadMascota = (double)mascota.EdadRegistro;
            }

            //calcular 6 semanas a factor de año
            double tiempoMinimoPrimeraVacunacion = 1.5 / 12;

            foreach (var vacunaVM in vacunasVM)
            {
                if (CalcularAnios((DateTime)vacunaVM.FechaVacunacion) > Math.Round((edadMascota - tiempoMinimoPrimeraVacunacion), 2, MidpointRounding.ToZero))
                {
                    foreach (var vacVM in vacunasVM)
                    {
                        ImagenCloudinary.EliminarImagenHosting(_cloudinary, vacVM.Imagen);
                    }
                    return BadRequest(new { mensaje = "La fecha de vacunación no puede ser una fecha anterior a las 6 semanas de vida de la mascota" });
                }
            }

            //validar fecha maxima de vacunacion
            if (vacunasVM.Where(v => v.FechaVacunacion > DateTime.Now.Date).Any())
            {
                foreach (var vacVM in vacunasVM)
                {
                    ImagenCloudinary.EliminarImagenHosting(_cloudinary, vacVM.Imagen);
                }
                return BadRequest(new { mensaje = "La fecha de vacunación no puede ser una fecha futura" });
            }

            //validar que vacuna obligatoria tiene imagen de verificacion asociada
            foreach (var vacunaVM in vacunasVM)
            {
                //var vacuna = await _context.Vacunas.AsNoTracking().FirstOrDefaultAsync(v => v.Id == vacunaVM.CodigoVacuna);

                if (vacunaVM.Obligatoria && vacunaVM.Imagen == null)
                {
                    foreach (var vacVM in vacunasVM)
                    {
                        ImagenCloudinary.EliminarImagenHosting(_cloudinary, vacVM.Imagen);
                    }
                    return BadRequest(new { mensaje = "No se cargo imagen de verificacion en vacunas obligatorias" });
                }
            }

            //Preparar registro de vacunas mascota
            if (!mascota.VacunaMascotas.Any())
            {
                foreach (var vacunaVM in vacunasVM)
                {
                    var vacunaMascota = new VacunaMascota
                    {
                        CodigoMascota = (int)vacunaVM.CodigoMascota,
                        CodigoVacuna = (int)vacunaVM.CodigoVacuna,
                        FechaVacunacion = (DateTime)vacunaVM.FechaVacunacion,
                        Imagen = vacunaVM.Imagen
                    };

                    mascota.VacunaMascotas.Add(vacunaMascota);
                }
            }
            else
            {
                foreach (var vacunaVM in vacunasVM)
                {
                    var vacunaMascota = mascota.VacunaMascotas
                        .FirstOrDefault(vm => vm.CodigoMascota == vacunaVM.CodigoMascota && vm.CodigoVacuna == vacunaVM.CodigoVacuna);

                    if (vacunaMascota == null)
                    {
                        vacunaMascota = new VacunaMascota
                        {
                            CodigoMascota = (int)vacunaVM.CodigoMascota,
                            CodigoVacuna = (int)vacunaVM.CodigoVacuna,
                            FechaVacunacion = (DateTime)vacunaVM.FechaVacunacion,
                            Imagen = vacunaVM.Imagen
                        };

                        mascota.VacunaMascotas.Add(vacunaMascota);
                    }
                    else
                    {
                        vacunaMascota.FechaVacunacion = (DateTime)vacunaVM.FechaVacunacion;
                        vacunaMascota.Imagen = vacunaVM.Imagen;

                    }
                }
            }

            /*
            //registrar vacunas de la mascota opcion 2
            foreach (var vacunaMascota in vacunasMascota)
            {
                if (vacunaMascota.CodigoVacunaMascota == 0)
                {
                    _context.VacunaMascotas.Add(vacunaMascota);
                }
                
            }
            */

            //cambiar estado mascota si es que tiene todas sus vacunas obligatorias
            List<Vacuna> vacunasObligatorias = await _context.Vacunas.Where(v => v.Obligatoria == true).AsNoTracking().ToListAsync();

            if(vacunasObligatorias != null && vacunasObligatorias.Count > 0)
            {
                foreach (var vacunaOb in vacunasObligatorias)
                {                    
                    if(mascota.VacunaMascotas.Where(vcm => vcm.CodigoVacuna == vacunaOb.Id).Any())
                    {
                        vacunasObligatorias.Remove(vacunaOb);
                    }
                }

                if(vacunasObligatorias.Count == 0)
                {
                    mascota.CodigoEstadoMascota = 2;
                }

            }

            //registrar vacunas de la mascota
            var registroVacunas = await _context.SaveChangesAsync();

            if (registroVacunas <= 0)
            {
                foreach (var vacVM in vacunasVM)
                {
                    ImagenCloudinary.EliminarImagenHosting(_cloudinary, vacVM.Imagen);
                }
                return BadRequest(new { mensaje = "No se pudo registrar las vacunas de la mascota" });
            }

            return Ok(new { mensaje = "Las vacunas de la mascota se registaron con éxito" });

        }


        private double CalcularAnios(DateTime fechaNacimiento)
        {
            TimeSpan diferenciaTiempo = DateTime.Now.Date - fechaNacimiento;

            double anios = diferenciaTiempo.TotalDays / 365.2425;

            if (anios < 1)
            {
                return Math.Round(anios, 2, MidpointRounding.ToZero);
            }
            else
            {
                return Math.Truncate(anios);
            }

        }
    }
}
