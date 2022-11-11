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
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using VoltaPetsAPI.Models.ViewModels;

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

        [HttpPost]
        [Route("Registrar")]
        [AllowAnonymous]       
        public async Task<IActionResult> RegistrarTutor([FromBody] UserTutor userTutor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //USUARIO

            //validar que no existe el email
            if (await _context.Usuarios.Where(u => u.Email == userTutor.Email).AnyAsync())
            {
                return BadRequest(new { mensaje = $"El Correo {userTutor.Email} ya tiene un usuario asociado" });
            }

            //crear usuario
            var usuario = new Usuario
            {
                Email = userTutor.Email,
                Password = Encriptacion.GetSHA256(userTutor.Password),
                CodigoRol = 3
            };

            //UBICACION

            //veriicar que se recibio un codigo comuna
            if (userTutor.CodigoComuna == 0)
            {
                return BadRequest(new { mensaje = "Se requiere codigo comuna" });
            }

            //Formatear departamento no recibido
            if (userTutor.Departamento == 0)
            {
                userTutor.Departamento = null;
            }

            //Buscar ubicacion en la base de datos
            // Agregar validacion de latitud y longitud
            var ubicacion = await _context.Ubicaciones
                .FirstOrDefaultAsync(ub => ub.CodigoComuna == userTutor.CodigoComuna && ub.Direccion.Equals(userTutor.Direccion) && ub.Departamento.Equals(userTutor.Departamento));

            //verificar que no existe la ubicacion
            if (ubicacion == null)
            {
                //ELIMINAR DESPUES DE USAR API ---------------------------------------------------------
                Random rand = new Random();

                ubicacion = new Ubicacion
                {
                    Direccion = userTutor.Direccion,
                    Departamento = userTutor.Departamento,
                    Latitud = rand.NextDouble(),                       // ELIMINAR
                    Longitud = rand.NextDouble(),                      //ELIMINAR
                    CodigoComuna = userTutor.CodigoComuna
                };
                
            }          

            //Tutor

            //registrar tutor
            var tutor = new Tutor
            {
                Nombre = userTutor.Nombre,
                Apellido = userTutor.Apellido,
                Telefono = userTutor.Telefono,
                Activado = true,
                Usuario = usuario,
                Ubicacion = ubicacion
            };

            _context.Tutores.Add(tutor);
            var registroTutor = await _context.SaveChangesAsync();

            //validar si no se pudo registrar el tutor
            if (registroTutor <= 0)
            {
                return BadRequest(new { mensaje = "No se pudo registrar el tutor" });
                
            }

            return Ok(new
            {
                codigoTutor = tutor.CodigoTutor,
                mensaje = "Cuenta creada con éxito"
            });
        }

        [HttpGet]
        [Route("Perfil")]
        [Authorize(Policy = "Tutor")]
        public async Task<IActionResult> ObtenerPerfilActual()
        {
            //Obtener codigo usuario logeado
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
            var tutor = await _context.Tutores
                .Include(t => t.Usuario)
                .ThenInclude(u => u.Imagen)
                .Include(t => t.Ubicacion)
                .ThenInclude(ub => ub.Comuna)
                .ThenInclude(c => c.Provincia)
                .ThenInclude(pv => pv.Region)
                .Where(t => t.CodigoUsuario == codigoUsuario)
                .Select(t => new Tutor
                {
                    Nombre = t.Nombre,
                    Apellido = t.Apellido,
                    Telefono = t.Telefono,
                    Descripcion = t.Descripcion,
                    Usuario = new Usuario
                    {
                        Email = t.Usuario.Email,
                        Imagen = new Imagen
                        {
                            Url = t.Usuario.Imagen.Url,
                            Path = t.Usuario.Imagen.Path
                        }
                        
                    },
                    Ubicacion = new Ubicacion
                    {
                        Direccion = t.Ubicacion.Direccion,
                        Departamento = t.Ubicacion.Departamento,
                        Comuna = new Comuna
                        {
                            CodigoComuna = t.Ubicacion.Comuna.CodigoComuna,
                            Descripcion = t.Ubicacion.Comuna.Descripcion,
                            Provincia = new Provincia
                            {
                                Region = new Region
                                {
                                    CodigoRegion = t.Ubicacion.Comuna.Provincia.Region.CodigoRegion,
                                    Descripcion = t.Ubicacion.Comuna.Provincia.Region.Descripcion
                                }
                            }
                        }
                    }
                }).FirstOrDefaultAsync();

            if(tutor == null)
            {
                return NotFound(new { mensaje = "No se pudo encontrar el Tutor" });
            }

            return Ok(new
            {
                Nombre = tutor.Nombre,
                Apellido = tutor.Apellido,
                Telefono = tutor.Telefono,
                Descripcion = tutor.Descripcion,
                Email = tutor.Usuario.Email,
                Imagen = tutor.Usuario.Imagen,
                Direccion = tutor.Ubicacion.Direccion,
                Departamento = tutor.Ubicacion.Departamento,
                CodigoComuna = tutor.Ubicacion.Comuna.CodigoComuna,
                Comuna = tutor.Ubicacion.Comuna.Descripcion,
                CodigoRegion = tutor.Ubicacion.Comuna.Provincia.Region.CodigoRegion,
                Region = tutor.Ubicacion.Comuna.Provincia.Region.Descripcion
            });

        }

        [HttpPut]
        [Route("EditarPerfil")]
        [Authorize(Policy = "Tutor")]
        public async Task<IActionResult> EditarPerfilActual(PerfilTutor perfil)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //obtener codigo usuario logeado
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
            var tutor = await _context.Tutores
                .Include(t => t.Usuario)
                .Include(t => t.Ubicacion)
                .FirstOrDefaultAsync(t => t.CodigoUsuario == codigoUsuario);

            if(tutor == null)
            {
                return NotFound(new { mensaje = "No se pudo encontrar el Tutor" });
            }

            //verificar si cambio la contraseña
            if (perfil.IsChangePassword)
            {
                if (!tutor.Usuario.Password.Equals(Encriptacion.GetSHA256(perfil.Password)))
                {
                    return BadRequest(new { mensaje = "La Contraseña actual es incorrecta" });
                }

                if (!perfil.NewPassword.Equals(perfil.ConfirmNewPassword))
                {
                    return BadRequest(new { mensaje = "Error en confirmar nueva contraseña" });
                }

                tutor.Usuario.Password = perfil.NewPassword;

            }

            //verfiicar si cambio la ubicacion
            if (!(tutor.Ubicacion.Direccion.Equals(perfil.Direccion) && tutor.Ubicacion.Departamento.Equals(perfil.Departamento) && tutor.Ubicacion.CodigoComuna == perfil.CodigoComuna))
            {
                //buscar ubicacion nueva en BD
                var ubicacionNueva = await _context.Ubicaciones.FirstOrDefaultAsync(ub => ub.Direccion.Equals(perfil.Direccion) && ub.Departamento.Equals(perfil.Departamento) && ub.CodigoComuna == perfil.CodigoComuna);

                //verificar si no existe la ubicacion en la BD
                if (ubicacionNueva == null)
                {
                    //API de GeoCordenadas -------------------------------------------
                    Random random = new Random();
                    double latitud = random.NextDouble();
                    double longitud = random.NextDouble();

                    ubicacionNueva = new Ubicacion
                    {
                        Direccion = perfil.Direccion,
                        Departamento = perfil.Departamento,
                        Latitud = latitud,
                        Longitud = longitud,
                        CodigoComuna = perfil.CodigoComuna
                    };
                    
                }

                tutor.Ubicacion = ubicacionNueva;

            }

            tutor.Telefono = perfil.Telefono;
            tutor.Descripcion= perfil.Descripcion;

            await _context.SaveChangesAsync();

            return NoContent();

        }







    }
}
