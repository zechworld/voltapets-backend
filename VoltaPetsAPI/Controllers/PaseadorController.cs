using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using VoltaPetsAPI.Data;
using VoltaPetsAPI.Helpers;
using VoltaPetsAPI.Models;
using VoltaPetsAPI.Models.User;

namespace VoltaPetsAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PaseadorController : ControllerBase
    {
        private readonly VoltaPetsContext _context;

        public PaseadorController(VoltaPetsContext context)
        {
            _context = context;
        }

        [Route("Registrar")]
        [HttpPost]
        public async Task<IActionResult> RegistrarPaseador([FromBody] UserPaseador userPaseador)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //USUARIO

            if (await _context.Usuarios.Where(u => u.Email == userPaseador.Email).AnyAsync())
            {
                return BadRequest(new { mensaje = $"El Correo {userPaseador.Email} ya tiene un usuario asociado" });
            }

            var usuario = new Usuario
            {
                Email = userPaseador.Email,
                Password = Encriptacion.GetSHA256(userPaseador.Password),
                CodigoRol = 2
            };

            //Paseador Rut

            if (await _context.Paseadores.Where(p => p.Rut == userPaseador.ObtenerRut() && p.Dv == userPaseador.ObtenerDv()).AnyAsync())
            {
                return BadRequest(new { mensaje = $"El Rut {userPaseador.ObtenerRut() + "-" + userPaseador.ObtenerDv()} ya tiene un usuario asociado" });
            }

            //UBICACION
            if (userPaseador.CodigoComuna == 0)
            {
                return BadRequest(new { mensaje = "Se requiere codigo comuna" });
            }

            if (userPaseador.Departamento == 0)
            {
                userPaseador.Departamento = null;
            }

            // Agregar validacion de latitud y longitud
            var ubicacionExistente = await _context.Ubicaciones.FirstOrDefaultAsync(ub => ub.CodigoComuna == userPaseador.CodigoComuna && ub.Direccion == userPaseador.Direccion && ub.Departamento.Equals(userPaseador.Departamento));

            if (ubicacionExistente != null)
            {
                userPaseador.CodigoUbicacion = ubicacionExistente.CodigoUbicacion;
            }
            else
            {
                //ELIMINAR DESPUES DE USAR API ---------------------------------------------------------
                Random rand = new Random();

                var ubicacion = new Ubicacion
                {
                    Direccion = userPaseador.Direccion,
                    Departamento = userPaseador.Departamento,
                    Latitud = rand.NextDouble(),                       // ELIMINAR
                    Longitud = rand.NextDouble(),                      //ELIMINAR
                    CodigoComuna = userPaseador.CodigoComuna
                };

                _context.Ubicaciones.Add(ubicacion);
                var registroUbicacion = await _context.SaveChangesAsync();

                if (registroUbicacion > 0)
                {
                    var ubicacionRegistrada = await _context.Ubicaciones.FirstOrDefaultAsync(ub => ub.CodigoComuna == ubicacion.CodigoComuna && ub.Direccion == ubicacion.Direccion && ub.Departamento.Equals(ubicacion.Departamento));

                    if (ubicacionRegistrada != null)
                    {
                        userPaseador.CodigoUbicacion = ubicacionRegistrada.CodigoUbicacion;
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

            //Codigo usuario

            _context.Usuarios.Add(usuario);
            var registroUsuario = await _context.SaveChangesAsync();

            if (registroUsuario > 0)
            {
                var usuarioRegistrado = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == usuario.Email);

                if (usuarioRegistrado != null)
                {
                    userPaseador.CodigoUsuario = usuarioRegistrado.CodigoUsuario;
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

            //Paseador

            var paseador = new Paseador
            {
                Rut = userPaseador.ObtenerRut(),
                Dv = userPaseador.ObtenerDv(),
                Nombre = userPaseador.Nombre,
                Apellido = userPaseador.Apellido,
                Telefono = userPaseador.Telefono,
                Activado = false,
                CodigoUsuario = userPaseador.CodigoUsuario,
                CodigoUbicacion = userPaseador.CodigoUbicacion

            };

            _context.Paseadores.Add(paseador);
            var registroPaseador = await _context.SaveChangesAsync();

            if (registroPaseador <= 0)
            {
                var ubicacionDelete = await _context.Ubicaciones.FindAsync(userPaseador.CodigoUbicacion);
                var usuarioDelete = await _context.Usuarios.FindAsync(userPaseador.CodigoUsuario);

                if (usuarioDelete != null && ubicacionDelete != null)
                {
                    _context.Ubicaciones.Remove(ubicacionDelete);
                    _context.Usuarios.Remove(usuarioDelete);

                    await _context.SaveChangesAsync();

                    return BadRequest(new { mensaje = "No se pudo registrar el tutor" });
                }
                else
                {
                    return BadRequest(new { mensaje = "No se pudo regisrar el tutor pero se registro usuario y ubicacion" });
                }
            }

            return Ok(new 
            {
                codigoUsuario = paseador.CodigoUsuario,
                mensaje = "Cuenta creada con éxito" 
            });

        }
    }
}
