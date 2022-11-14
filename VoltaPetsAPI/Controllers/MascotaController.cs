using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using VoltaPetsAPI.Data;
using VoltaPetsAPI.Models;
using VoltaPetsAPI.Models.ViewModels;

namespace VoltaPetsAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class MascotaController : ControllerBase
    {
        private readonly VoltaPetsContext _context;

        public MascotaController(VoltaPetsContext context)
        {
            _context = context;
        }

        
        [HttpPost]
        [Route("Registrar")]
        [Authorize(Policy = "Tutor")]
        
        public  async Task<IActionResult> RegistrarMascota(MascotaVM mascotaVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //validar fecha nacimiento o adopcion mascota
            DateTime fechaMaximaPermitida = DateTime.Now - TimeSpan.FromDays(180);

            if (mascotaVM.FechaNacimiento >= fechaMaximaPermitida)
            {
                return BadRequest(new { mensaje = "No se permiten mascotas menores a 6 meses de edad" });
            }

            DateTime fechaMinimaPermitida = DateTime.Now - TimeSpan.FromDays(10950);

            if(mascotaVM.FechaNacimiento <= fechaMinimaPermitida)
            {
                return BadRequest(new { mensaje = "Ingresa una fecha de nacimiento real (No es posible que un perro tenga más de 30 años)" });
            }

            //validar edad mascota
            var tiempoMascota = CalcularAnios(mascotaVM.FechaNacimiento);

            if (mascotaVM.IsFechaNacimiento && mascotaVM.EdadRegistro > 0 && mascotaVM.EdadRegistro != tiempoMascota)
            {
                return BadRequest(new { mensaje = "La edad ingresada no coincide con la edad calculada desde la fecha de nacimiento (La edad no es obligatoria si ingresa una fecha de nacimiento) " });
            }

            if (!mascotaVM.IsFechaNacimiento && mascotaVM.EdadRegistro == 0)
            {
                return BadRequest(new { mensaje = "La fecha de adopcion requiere que ingrese la edad de la mascota para poder actualizarla automáticamente" });
            }

            if (!mascotaVM.IsFechaNacimiento && mascotaVM.EdadRegistro < tiempoMascota)
            {
                return BadRequest(new { mensaje = $"La edad ingresada no puede ser menor a {tiempoMascota} años, correspondiente al tiempo de adopcion" });
            }            

            if (mascotaVM.IsFechaNacimiento)
            {
                mascotaVM.EdadRegistro = null;
            }

            //obtener grupo etario
            GrupoEtario grupoEtario;

            if (mascotaVM.IsFechaNacimiento)
            {
                grupoEtario = await _context.GrupoEtarios.FirstOrDefaultAsync(gpe => gpe.EdadInferior <= tiempoMascota && gpe.EdadSuperior > tiempoMascota);
                
                if (grupoEtario == null)
                {
                    return NotFound(new { mensaje = "No se pudo obtener el grupo etario de la mascota" });
                }
                
            }
            else
            {
                grupoEtario = await _context.GrupoEtarios.FirstOrDefaultAsync(gpe => gpe.EdadInferior <= (double)mascotaVM.EdadRegistro && gpe.EdadSuperior > (double)mascotaVM.EdadRegistro);

                if (grupoEtario == null)
                {
                    return NotFound(new { mensaje = "No se pudo obtener el grupo etario de la mascota" });
                }
                                
            }
            

            //Obtener usuario logeado
            var claims = (ClaimsIdentity)User.Identity;
            var codUser = claims.FindFirst(JwtRegisteredClaimNames.Sid).Value;
            int codigoUsuario;

            if (int.TryParse(codUser, out int id))
            {
                codigoUsuario = id;
            }
            else
            {
                return BadRequest(new { mensaje = "Error en obtener el codigo del usuario actual" });
            }

            //obtener tutor asociado al usuario
            var tutor = await _context.Tutores.FirstOrDefaultAsync(t => t.CodigoUsuario == codigoUsuario);

            if(tutor == null)
            {
                return NotFound(new { mensaje = "No se pudo encontrar el tutor de la mascota" });
            }
            
            //registrar mascota
            Mascota mascota = new Mascota
            {
                Nombre = mascotaVM.Nombre,
                Descripcion = mascotaVM.Descripcion,
                Esterilizado = mascotaVM.Esterilizado,
                FechaNacimiento = mascotaVM.FechaNacimiento,
                EdadRegistro = mascotaVM.EdadRegistro,
                CodigoTutor = tutor.CodigoTutor,
                CodigoRaza = (int)mascotaVM.CodigoRaza,
                CodigoTamanio = (int)mascotaVM.CodigoTamanio,
                CodigoSexo = (int)mascotaVM.CodigoSexo,
                CodigoEtario = grupoEtario.CodigoEtario,
                CodigoEstadoMascota = 1
            };
            
            _context.Mascotas.Add(mascota);
            var registroMascota = await _context.SaveChangesAsync();

            //validar si no se pudo registrar la mascota
            if (registroMascota <= 0)
            {
                return BadRequest(new { mensaje = "No se pudo registrar la mascota" });

            }

            return Ok(new
            {
                codigoMascota = mascota.CodigoMascota,
                mensaje = "Mascota registrada con éxito"
            });
            

        }
        

        
        private double CalcularAnios(DateTime fechaNacimiento)
        {
            TimeSpan diferenciaTiempo = DateTime.Now.Date - fechaNacimiento;

            double anios = diferenciaTiempo.TotalDays / 365.2425;

            if (anios < 1)
            {
                return Math.Round(anios,2,MidpointRounding.ToZero);
            }
            else
            {
                return Math.Truncate(anios);
            }           

        }

        
        /*
        
        private int CalcularEdad(DateTime fechaNacimiento)
        {
            //Obtener diferencias entre la fecha actual y la fecha de nacimiento
            int diferenciaYear = DateTime.Now.Year - fechaNacimiento.Year;
            int diferenciaMes = DateTime.Now.Month - fechaNacimiento.Month;
            int diferenciaDia = DateTime.Now.Day - fechaNacimiento.Day;

            //Si la diferencia de mes es negativa significa que el mes actual es anterior al mes de nacimiento
            if (diferenciaMes < 0)
            {
                //Como aun no se ha alcanzado el mes de nacimiento, la edad es la diferencia de anios menos uno
                return diferenciaYear - 1;
            }

            //Si la diferencia de mes es igual a cero significa que el mes actual es el mes de nacimiento
            if (diferenciaMes == 0)
            {
                //Si la diferencia de dias es negativa significa que el dia actual es anterior al dia de nacimiento (No se ha cumplido anios)
                return diferenciaDia < 0 ? diferenciaYear - 1 : diferenciaYear;
            }

            //Si no se cumplen las condiciones anteriores, el mes es mayor a cero, por lo que el mes actual ha pasado al mes de nacimiento, es decir, se cumplieron anios
            return diferenciaYear;

        }

        */





    }
}
