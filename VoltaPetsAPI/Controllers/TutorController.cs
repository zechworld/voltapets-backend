using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using VoltaPetsAPI.Data;
using VoltaPetsAPI.Helpers;
using VoltaPetsAPI.Models.User;
using VoltaPetsAPI.Models;
using System;

namespace VoltaPetsAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TutorController : ControllerBase
    {
        private readonly VoltaPetsContext _context;

        public TutorController(VoltaPetsContext context)
        {
            _context = context;
        }

        [Route("Registrar")]
        [HttpPost]
        public async Task<IActionResult> RegistrarTutor([FromBody] UserTutor userTutor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //USUARIO

            if (await _context.Usuarios.Where(u => u.Email == userTutor.Email).AnyAsync())
            {
                return BadRequest(new { mensaje = $"El Correo {userTutor.Email} ya tiene un usuario asociado" });
            }

            var usuario = new Usuario
            {
                Email = userTutor.Email,
                Password = Encriptacion.GetSHA256(userTutor.Password),
                CodigoRol = 3
            };

            //UBICACION
            if (userTutor.CodigoComuna == 0)
            {
                return BadRequest(new { mensaje = "Se requiere codigo comuna" });
            }


            if (userTutor.Departamento == 0)
            {
                userTutor.Departamento = null;
            }


            // Agregar validacion de latitud y longitud
            var ubicacionBD = await _context.Ubicaciones.FirstOrDefaultAsync(ub => ub.CodigoComuna == userTutor.CodigoComuna && ub.Direccion == userTutor.Direccion && ub.Departamento.Equals(userTutor.Departamento));

            if (ubicacionBD != null)
            {
                userTutor.CodigoUbicacion = ubicacionBD.CodigoUbicacion;
            }
            else
            {
                //ELIMINAR DESPUES DE USAR API ---------------------------------------------------------
                Random rand = new Random();

                var ubicacion = new Ubicacion
                {
                    Direccion = userTutor.Direccion,
                    Departamento = userTutor.Departamento,
                    Latitud = rand.NextDouble(),                       // ELIMINAR
                    Longitud = rand.NextDouble(),                      //ELIMINAR
                    CodigoComuna = userTutor.CodigoComuna
                };

                _context.Ubicaciones.Add(ubicacion);
                var registroUbicacion = await _context.SaveChangesAsync();

                if (registroUbicacion > 0)
                {
                    var ubicacionDB = await _context.Ubicaciones.FirstOrDefaultAsync(ub => ub.CodigoComuna == ubicacion.CodigoComuna && ub.Direccion == ubicacion.Direccion && ub.Departamento.Equals(ubicacion.Departamento));

                    if (ubicacionDB != null)
                    {
                        userTutor.CodigoUbicacion = ubicacionDB.CodigoUbicacion;
                    }
                    else
                    {
                        return BadRequest(new { mensaje = "No se pudo encontrar el codigo de la ubicacion insertada" });
                    }
                }
                else
                {
                    return BadRequest(new { mensaje = "No se pudo insertar la ubicacion" });
                }
            }

            _context.Usuarios.Add(usuario);
            var registroUsuario = await _context.SaveChangesAsync();

            if (registroUsuario > 0)
            {
                var usuarioDB = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == usuario.Email);

                if (usuarioDB != null)
                {
                    userTutor.CodigoUsuario = usuarioDB.CodigoUsuario;
                }
                else
                {
                    return BadRequest(new { mensaje = "No se pudo encontrar el codigo del usuario insertado" });
                }

            }
            else
            {
                return BadRequest(new { mensaje = "No se pudo insertar el usuario" });
            }

            //Tutor

            var tutor = new Tutor
            {
                Nombre = userTutor.Nombre,
                Apellido = userTutor.Apellido,
                Telefono = userTutor.Telefono,
                Activado = true,
                CodigoUsuario = userTutor.CodigoUsuario,
                CodigoUbicacion = userTutor.CodigoUbicacion
            };

            _context.Tutores.Add(tutor);
            var registroTutor = await _context.SaveChangesAsync();

            if (registroTutor <= 0)
            {
                var ubicacionDelete = await _context.Ubicaciones.FindAsync(userTutor.CodigoUbicacion);
                var usuarioDelete = await _context.Usuarios.FindAsync(userTutor.CodigoUsuario);

                if (usuarioDelete != null && ubicacionDelete != null)
                {
                    _context.Ubicaciones.Remove(ubicacionDelete);
                    _context.Usuarios.Remove(usuarioDelete);

                    await _context.SaveChangesAsync();

                    return BadRequest(new { mensaje = "No se pudo registrar el tutor" });
                }
                else
                {
                    return BadRequest(new { mensaje = "No se pudo registrar el tutor pero se registro usuario y ubicacion" });
                }
            }

            return Created();
        }
    }
}
