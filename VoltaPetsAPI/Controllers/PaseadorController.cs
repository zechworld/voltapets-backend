using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using VoltaPetsAPI.Data;
using VoltaPetsAPI.Helpers;
using VoltaPetsAPI.Models;
using VoltaPetsAPI.Models.User;
using VoltaPetsAPI.Models.ViewModels;

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

        [HttpPost]
        [Route("Registrar")]
        [AllowAnonymous]        
        public async Task<IActionResult> RegistrarPaseador([FromBody] UserPaseador userPaseador)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //USUARIO

            //validar que no exista email
            if (await _context.Usuarios.Where(u => u.Email == userPaseador.Email).AnyAsync())
            {
                return BadRequest(new { mensaje = $"El Correo {userPaseador.Email} ya tiene un usuario asociado" });
            }

            //crear usuario
            var usuario = new Usuario
            {
                Email = userPaseador.Email,
                Password = Encriptacion.GetSHA256(userPaseador.Password),
                CodigoRol = 2
            };

            //validar que no exista el Rut del paseador
            if (await _context.Paseadores.Where(p => p.Rut == userPaseador.ObtenerRut() && p.Dv == userPaseador.ObtenerDv()).AnyAsync())
            {
                return BadRequest(new { mensaje = $"El Rut {userPaseador.ObtenerRut() + "-" + userPaseador.ObtenerDv()} ya tiene un usuario asociado" });
            }

            //UBICACION

            //Verificar que se recibio un codigo comuna
            if (userPaseador.CodigoComuna == 0)
            {
                return BadRequest(new { mensaje = "Se requiere codigo comuna" });
            }

            //Formatear departamento no recibido
            if (userPaseador.Departamento == 0)
            {
                userPaseador.Departamento = null;
            }                       

            //Buscar ubicacion en la BD            
            var ubicacion = await _context.Ubicaciones
                .FirstOrDefaultAsync(ub => ub.CodigoComuna == userPaseador.CodigoComuna && ub.Direccion.Equals(userPaseador.Direccion) && ub.Departamento.Equals(userPaseador.Departamento)); // Agregar validacion de latitud y longitud

            //Verificar si no existe la ubicacion
            if(ubicacion == null)
            {
                Random rand = new Random(); //ELIMINAR DESPUES DE USAR API ---------------------------------
                ubicacion = new Ubicacion
                {
                    Direccion = userPaseador.Direccion,
                    Departamento = userPaseador.Departamento,
                    Latitud = rand.NextDouble(),                       // ELIMINAR
                    Longitud = rand.NextDouble(),                      //ELIMINAR
                    CodigoComuna = userPaseador.CodigoComuna
                };
            }

            //Paseador

            //registrar paseador
            var paseador = new Paseador
            {
                Rut = userPaseador.ObtenerRut(),
                Dv = userPaseador.ObtenerDv(),
                Nombre = userPaseador.Nombre,
                Apellido = userPaseador.Apellido,
                Telefono = userPaseador.Telefono,
                Activado = false,
                Usuario = usuario,
                Ubicacion = ubicacion

            };               

            _context.Paseadores.Add(paseador);
            var registroPaseador = await _context.SaveChangesAsync();

            //validar si no se registro el paseador
            if (registroPaseador <= 0)
            {
                return BadRequest(new { mensaje = "No se pudo registrar el paseador" });
                
            }

            return Ok(new
            {
                codigoUsuario = paseador.CodigoUsuario,
                mensaje = "Cuenta creada con éxito"
            });

        }

        [HttpGet]
        [Route("Perfil")]
        [Authorize(Policy = "Paseador")]
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

            /*
            var usuario = await _context.Usuarios.FindAsync(codigoUsuario);

            if(usuario == null)
            {
                return NotFound(new {mensaje = "No se pudo encontrar el Usuario" });
            }
            */

            //obtener paseador asociado al usuario
            var paseador = await _context.Paseadores
                .Include(p => p.Usuario)
                .ThenInclude(u => u.Imagen)
                .Include(p => p.ExperienciaPaseador)
                .Include(p => p.Ubicacion)
                .ThenInclude(ub => ub.Comuna)
                .ThenInclude(c => c.Provincia)
                .ThenInclude(pv => pv.Region)
                .Where(p => p.CodigoUsuario == codigoUsuario)
                .Select(p => new Paseador
                {
                    Nombre = p.Nombre,
                    Apellido = p.Apellido,
                    Descripcion = p.Descripcion,
                    Telefono = p.Telefono,
                    Usuario = new Usuario
                    {
                        Email = p.Usuario.Email
                    },
                    ExperienciaPaseador = new ExperienciaPaseador
                    {
                        Descripcion = p.ExperienciaPaseador.Descripcion
                    },
                    Ubicacion = new Ubicacion
                    {
                        Direccion = p.Ubicacion.Direccion,
                        Departamento = p.Ubicacion.Departamento,
                        Comuna = new Comuna
                        {
                            CodigoComuna= p.Ubicacion.Comuna.CodigoComuna,
                            Descripcion = p.Ubicacion.Comuna.Descripcion,
                            Provincia = new Provincia
                            {
                                Region = new Region
                                {
                                    CodigoRegion = p.Ubicacion.Comuna.Provincia.Region.CodigoRegion,
                                    Descripcion = p.Ubicacion.Comuna.Provincia.Region.Descripcion
                                }
                            }
                        }
                    }
                })
                .FirstOrDefaultAsync();

            if (paseador == null)
            {
                return NotFound(new { mensaje = "No se pudo encontrar el Paseador" });
            }

            /*
            //obtener ubicacion del paseador
            var ubicacion = await _context.Ubicaciones.FindAsync(paseador.CodigoUbicacion);

            if(ubicacion == null)
            {
                return NotFound(new { mensaje = "No se pudo encontrar la ubicacion del paseador" });
            }

            
            //obtener comuna del paseador
            var comuna = await _context.Comunas.FindAsync(ubicacion.CodigoComuna);

            if(comuna == null)
            {
                return NotFound(new { mensaje = "No se pudo encontrar la comuna del paseador" });
            }

            //obtener provincia del paseador
            var provincia = await _context.Provincias.FindAsync(comuna.CodigoProvincia);

            if(provincia == null)
            {
                return NotFound(new { mensaje = "no se pudo obtener la provincia del paseador" });
            }

            //obtener region del paseador
            var region = await _context.Regiones.FindAsync(provincia.CodigoRegion);

            if(region == null)
            {
                return NotFound(new { mensaje = "no se pudo obtener la region del paseador" });
            }

            //obtener experiencia paseador
            ExperienciaPaseador experiencia;

            if (paseador.CodigoExperiencia.Equals(null))
            {
                experiencia = new ExperienciaPaseador() 
                { 
                    CodigoExperiencia = 0,
                    Descripcion = null
                };
            }
            else
            {
                experiencia = await _context.ExperienciaPaseadores.FindAsync(paseador.CodigoExperiencia);

                if(experiencia == null)
                {
                    return NotFound(new { mensaje = "no se pudo obtener la experiencia del paseador" });
                }

            }

            */
            //Calificacion

            float calificacion = 0;

            //Revisar si existen calificaciones
            if (await _context.Paseos.Where(ps => ps.CodigoPaseador == paseador.CodigoPaseador && ps.Calificado).AsNoTracking().AnyAsync())
            {
                //obtener calificacion paseador
                calificacion = await _context.Paseos
                .Include(ps => ps.Calificacion)
                .Where(ps => ps.CodigoPaseador == paseador.CodigoPaseador && ps.Calificado)
                .AverageAsync(ps => ps.Calificacion.Valor);
            }        

            return Ok(new 
            { 
                Nombre = paseador.Nombre,
                Apellido = paseador.Apellido,
                Descripcion = paseador.Descripcion,
                Telefono = paseador.Telefono,
                Email = paseador.Usuario.Email,
                Imagen = paseador.Usuario.Imagen,
                Direccion = paseador.Ubicacion.Direccion,
                Departamento = paseador.Ubicacion.Departamento,
                CodigoComuna = paseador.Ubicacion.Comuna.CodigoComuna,
                Comuna = paseador.Ubicacion.Comuna.Descripcion,
                CodigoRegion = paseador.Ubicacion.Comuna.Provincia.Region.CodigoRegion,
                Region = paseador.Ubicacion.Comuna.Provincia.Region.Descripcion,
                Experencia = paseador.ExperienciaPaseador.Descripcion,
                Calificacion = calificacion

            });

        }

        [HttpPut]
        [Route("EditarPerfil")]
        [Authorize(Policy = "Paseador")]
        public async Task<IActionResult> EditarPerfilActual(PerfilPaseador perfil)
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

            /*
            var usuario = await _context.Usuarios.FindAsync(codigoUsuario);

            if (usuario == null)
            {
                return NotFound(new { mensaje = "No se pudo encontrar el Usuario" });
            }
            */

            //obtener paseador asociado al usuario
            var paseador = await _context.Paseadores
                .Include(p => p.Usuario)
                .Include(p => p.Ubicacion)
                .FirstOrDefaultAsync(p => p.CodigoUsuario == codigoUsuario);

            if (paseador == null)
            {
                return NotFound(new { mensaje = "No se pudo encontrar el Paseador" });
            }

            /*

            //obtener ubicacion actual del paseador
            var ubicacionActual = await _context.Ubicaciones.FindAsync(paseador.CodigoUbicacion);

            if(ubicacionActual == null)
            {
                return NotFound(new { mensaje = "No se pudo encontrar la ubicacion actual del paseador" });
            }

            */

            // Contraseña

            if (perfil.IsChangePassword)
            {
                if (!paseador.Usuario.Password.Equals(Encriptacion.GetSHA256(perfil.Password)))
                {
                    return BadRequest(new { mensaje = "La Contraseña actual es incorrecta" });
                }

                if (!perfil.NewPassword.Equals(perfil.ConfirmNewPassword))
                {
                    return BadRequest(new { mensaje = "Error en confirmar nueva contraseña" });
                }

                paseador.Usuario.Password = perfil.NewPassword;

            }

            //  Ubicacion

            //Verificar si Cambio de ubicacion
            if (!(paseador.Ubicacion.Direccion.Equals(perfil.Direccion) && paseador.Ubicacion.Departamento.Equals(perfil.Departamento) && paseador.Ubicacion.CodigoComuna == perfil.CodigoComuna))
            {
                /* 
                verificar ubicacion nueva existe

                if(await _context.Ubicaciones.Where(ub => ub.Direccion.Equals(perfil.Direccion) && ub.Departamento.Equals(perfil.Departamento) && ub.CodigoComuna == perfil.CodigoComuna).AnyAsync())
                {
                    
                }
                */

                //Buscar ubicacion nueva en base de datos
                var ubicacionNueva = await _context.Ubicaciones
                    .FirstOrDefaultAsync(ub => ub.Direccion.Equals(perfil.Direccion) && ub.Departamento.Equals(perfil.Departamento) && ub.CodigoComuna == perfil.CodigoComuna);

                //Verificar si no existe la ubicacion nueva en la base de datos
                if(ubicacionNueva == null)
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

                    /*

                    //insertar ubicacion nueva
                    var ubicacionNueva = new Ubicacion()
                    {
                        Direccion = perfil.Direccion,
                        Departamento = perfil.Departamento,
                        Latitud = latitud,
                        Longitud = longitud,
                        CodigoComuna = perfil.CodigoComuna
                    };

                    _context.Ubicaciones.Add(ubicacionNueva);
                    await _context.SaveChangesAsync();

                    */

                }

                paseador.Ubicacion = ubicacionNueva;

            }

            paseador.Telefono = perfil.Telefono;
            paseador.Descripcion = perfil.Descripcion;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet]
        [Route("Laboral")]
        [Authorize(Policy = "Paseador")]
        public  async Task<IActionResult> ObtenerParametrosLaboralesActual()
        {
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

            /*
            var usuario = await _context.Usuarios.FindAsync(codigoUsuario);

            if (usuario == null)
            {
                return NotFound(new { mensaje = "No se pudo encontrar el Usuario" });
            }
            */

            //obtener paseador asociado al usuario
            var paseador = await _context.Paseadores
                .Include(p => p.PerroAceptado)
                .FirstOrDefaultAsync(p => p.CodigoUsuario == codigoUsuario);

            if (paseador == null)
            {
                return NotFound(new { mensaje = "No se pudo encontrar el Paseador" });
            }

            
            //obtener tarifa
            var tarifa = await _context.Tarifas
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.CodigoPaseador == paseador.CodigoPaseador && t.FechaTermino.Equals(null));

            if(tarifa == null)
            {
                tarifa = new Tarifa()
                {
                    Basico = 0,
                    Juego = 0,
                    Social = 0

                };
            }

            /*
            //obtener perro aceptado
            var aceptado = await _context.PerroAceptados
                .AsNoTracking()
                .FirstOrDefaultAsync(pa => pa.CodigoPaseador == paseador.CodigoPaseador);
            */

            if(paseador.PerroAceptado == null)
            {
                paseador.PerroAceptado = new PerroAceptado()
                {
                    TamanioToy = false,
                    TamanioPequenio = false,
                    TamanioMediano = false,
                    TamanioGrande = false,
                    TamanioGigante = false,
                    CantidadPerro = 0
                };
            }
            

            return Ok(new
            {
                Basico = tarifa.Basico,
                Juego = tarifa.Juego,
                Social = tarifa.Social,
                Toy = paseador.PerroAceptado.TamanioToy,
                Pequenio = paseador.PerroAceptado.TamanioPequenio,
                Mediano = paseador.PerroAceptado.TamanioMediano,
                Grande = paseador.PerroAceptado.TamanioGrande,
                Gigante = paseador.PerroAceptado.TamanioGigante,
                cantidad = paseador.PerroAceptado.CantidadPerro

            });
            
        }

        /*
        [Route("EditarLaboral")]
        [HttpPut]
        public async Task<IActionResult> EditarParametrosLaboralesActual()
        {

        }
        */
        

        /*
        
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

        */



    }


}

