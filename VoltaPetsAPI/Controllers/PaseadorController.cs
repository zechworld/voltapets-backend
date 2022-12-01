using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ObtenerPaseadores()
        {
            return Ok(await _context.Paseadores.Where(p => p.Activado == true).ToListAsync());
        }

        [HttpGet]
        [Route("{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> ObtenerPaseador(int id)
        {
            var paseador = await _context.Paseadores
                .Include(p => p.PerroAceptado)
                .Include(p => p.Tarifas)
                .FirstOrDefaultAsync(paseador => paseador.Id == id);

            if (paseador == null)
            {
                return NotFound(new { mensaje = "No se ha podido encontrar el paseador" });
            }

            return Ok(paseador);
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
                Ubicacion = ubicacion,
                CodigoExperiencia = 1

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
                        Email = p.Usuario.Email,
                        Imagen = new Imagen
                        {
                            Url = p.Usuario.Imagen.Url,
                            Path = p.Usuario.Imagen.Path
                        }
                        
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
                            Id = p.Ubicacion.Comuna.Id,
                            Descripcion = p.Ubicacion.Comuna.Descripcion,
                            Provincia = new Provincia
                            {
                                Region = new Region
                                {
                                    Id = p.Ubicacion.Comuna.Provincia.Region.Id,
                                    Descripcion = p.Ubicacion.Comuna.Provincia.Region.Descripcion
                                }
                            }
                        }
                    }
                }).AsNoTracking()
                .FirstOrDefaultAsync();

            if (paseador == null)
            {
                return NotFound(new { mensaje = "No se pudo encontrar el Paseador" });
            }

            //Calificacion

            float calificacion = 0;

            //Revisar si existen calificaciones
            if (await _context.Paseos.Where(ps => ps.CodigoPaseador == paseador.Id && ps.Calificado).AsNoTracking().AnyAsync())
            {
                //obtener calificacion paseador
                calificacion = await _context.Paseos
                .Include(ps => ps.Calificacion)
                .Where(ps => ps.CodigoPaseador == paseador.Id && ps.Calificado)
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
                CodigoComuna = paseador.Ubicacion.Comuna.Id,
                Comuna = paseador.Ubicacion.Comuna.Descripcion,
                CodigoRegion = paseador.Ubicacion.Comuna.Provincia.Region.Id,
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

                paseador.Usuario.Password = Encriptacion.GetSHA256(perfil.NewPassword);

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
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.CodigoUsuario == codigoUsuario);

            if (paseador == null)
            {
                return NotFound(new { mensaje = "No se pudo encontrar el Paseador" });
            }

            
            //obtener tarifa
            var tarifa = await _context.Tarifas
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.CodigoPaseador == paseador.Id && t.FechaTermino == null);

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

        [HttpGet]
        [Route("RestriccionLaboral")]
        [Authorize(Policy = "Paseador")]
        public async Task<IActionResult> ObtenerRestriccionesLaboralesActual()
        {
            var claims = (ClaimsIdentity)User.Identity;
            var codigoUsuario = UsuarioConectado.ObtenerCodigo(claims);

            if(codigoUsuario == 0)
            {
                return BadRequest(new { mensaje = "Error en obtener el codigo del usuario actual" });
            }

            var paseador = await _context.Paseadores
                .Include(p => p.ExperienciaPaseador)
                .ThenInclude(exp => exp.RangoTarifa)
                .Include(p => p.ExperienciaPaseador.PerroPermitido)
                .Where(p => p.CodigoUsuario == codigoUsuario)
                .Select(p => new Paseador
                {
                    ExperienciaPaseador = new ExperienciaPaseador
                    {
                        RangoTarifa = new RangoTarifa
                        {
                            BasicoInferior = p.ExperienciaPaseador.RangoTarifa.BasicoInferior,
                            BasicoSuperior = p.ExperienciaPaseador.RangoTarifa.BasicoSuperior,
                            JuegoInferior = p.ExperienciaPaseador.RangoTarifa.JuegoInferior,
                            JuegoSuperior = p.ExperienciaPaseador.RangoTarifa.JuegoSuperior,
                            SocialInferior = p.ExperienciaPaseador.RangoTarifa.SocialInferior,
                            SocialSuperior = p.ExperienciaPaseador.RangoTarifa.SocialSuperior
                        },
                        PerroPermitido = new PerroPermitido
                        {
                            TamanioMediano = p.ExperienciaPaseador.PerroPermitido.TamanioMediano,
                            TamanioGrande = p.ExperienciaPaseador.PerroPermitido.TamanioGrande,
                            TamanioGigante = p.ExperienciaPaseador.PerroPermitido.TamanioGigante
                        }
                    }
                })
                .AsNoTracking()
                .FirstOrDefaultAsync();
                
            if(paseador == null)
            {
                return NotFound(new { mensaje = "No se pudo obtener al paseador" });
            }

            return Ok(new
            {
                BasicoInferior = paseador.ExperienciaPaseador.RangoTarifa.BasicoInferior,
                BasicoSuperior = paseador.ExperienciaPaseador.RangoTarifa.BasicoSuperior,
                JuegoInferior = paseador.ExperienciaPaseador.RangoTarifa.JuegoInferior,
                JuegoSuperior = paseador.ExperienciaPaseador.RangoTarifa.JuegoSuperior,
                SocialInferior = paseador.ExperienciaPaseador.RangoTarifa.SocialInferior,
                SocialSuperior = paseador.ExperienciaPaseador.RangoTarifa.SocialSuperior,
                TamanioMediano = paseador.ExperienciaPaseador.PerroPermitido.TamanioMediano,
                TamanioGrande = paseador.ExperienciaPaseador.PerroPermitido.TamanioGrande,
                TamanioGigante = paseador.ExperienciaPaseador.PerroPermitido.TamanioGigante
            });

        }

        [HttpPut]
        [Route("EditarLaboral")]
        [Authorize(Policy = "Paseador")]
        public async Task<IActionResult> EditarParametrosLaboralesActual(ParametroLaboralVM parametros)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var claims = (ClaimsIdentity)User.Identity;
            var codigoUsuario = UsuarioConectado.ObtenerCodigo(claims);

            if (codigoUsuario == 0)
            {
                return BadRequest(new { mensaje = "Error en obtener el codigo del usuario actual" });
            }

            var paseador = await _context.Paseadores
                .Include(p => p.PerroAceptado)
                .Include(p => p.Tarifas)
                .Include(p => p.ExperienciaPaseador)
                .ThenInclude(exp => exp.RangoTarifa)
                .Include(p => p.ExperienciaPaseador.PerroPermitido)
                .Where(p => p.CodigoUsuario == codigoUsuario)
                .Select(p => new Paseador
                {
                    PerroAceptado = p.PerroAceptado,
                    Tarifas = p.Tarifas,
                    ExperienciaPaseador = new ExperienciaPaseador
                    {
                        RangoTarifa = new RangoTarifa
                        {
                            BasicoInferior = p.ExperienciaPaseador.RangoTarifa.BasicoInferior,
                            BasicoSuperior = p.ExperienciaPaseador.RangoTarifa.BasicoSuperior,
                            JuegoInferior = p.ExperienciaPaseador.RangoTarifa.JuegoInferior,
                            JuegoSuperior = p.ExperienciaPaseador.RangoTarifa.JuegoSuperior,
                            SocialInferior = p.ExperienciaPaseador.RangoTarifa.SocialInferior,
                            SocialSuperior = p.ExperienciaPaseador.RangoTarifa.SocialSuperior
                        },
                        PerroPermitido = new PerroPermitido
                        {
                            TamanioMediano = p.ExperienciaPaseador.PerroPermitido.TamanioMediano,
                            TamanioGrande = p.ExperienciaPaseador.PerroPermitido.TamanioGrande,
                            TamanioGigante = p.ExperienciaPaseador.PerroPermitido.TamanioGigante
                        }
                    }
                }).FirstOrDefaultAsync();

            if(paseador == null)
            {
                return NotFound(new { mensaje = "No se pudo obtener al paseador" });
            }

            if (!paseador.Activado)
            {
                return BadRequest(new { mensaje = "No se puede editar los parámtros laborales con una cuenta de Paseador no activada" });
            }

            //validar parametros con restricciones de experiencia paseador
            if(parametros.TamanioMediano != paseador.ExperienciaPaseador.PerroPermitido.TamanioMediano)
            {
                return BadRequest(new { mensaje = "Su experiencia actual le impide seleccionar el tamaño Mediano de mascota" });
            }

            if (parametros.TamanioGrande != paseador.ExperienciaPaseador.PerroPermitido.TamanioGrande)
            {
                return BadRequest(new { mensaje = "Su experiencia actual le impide seleccionar el tamaño Grande de mascota" });
            }

            if (parametros.TamanioGigante != paseador.ExperienciaPaseador.PerroPermitido.TamanioGigante)
            {
                return BadRequest(new { mensaje = "Su experiencia actual le impide seleccionar el tamaño Gigante de mascota" });
            }

            if (parametros.Basico < paseador.ExperienciaPaseador.RangoTarifa.BasicoInferior || parametros.Basico > paseador.ExperienciaPaseador.RangoTarifa.BasicoSuperior)
            {
                return BadRequest(new { mensaje = "La tarifa de paseo de necesidades básicas se encuentra fuera del rango permitido por su experiencia" });
            }

            if (parametros.Juego < paseador.ExperienciaPaseador.RangoTarifa.JuegoInferior || parametros.Juego > paseador.ExperienciaPaseador.RangoTarifa.JuegoSuperior)
            {
                return BadRequest(new { mensaje = "La tarifa de tiempo de juego se encuentra fuera del rango permitido por su experiencia" });
            }

            if (parametros.Social < paseador.ExperienciaPaseador.RangoTarifa.SocialInferior || parametros.Social > paseador.ExperienciaPaseador.RangoTarifa.SocialSuperior)
            {
                return BadRequest(new { mensaje = "La tarifa socialización con otras mascotas se encuentra fuera del rango permitido por su experiencia" });
            }

            //preparar datos para actualizar

            //PerroAceptado
            if(paseador.PerroAceptado == null)
            {
                PerroAceptado primerPerroAceptado = new PerroAceptado
                {
                    TamanioGigante = parametros.TamanioGigante,
                    TamanioGrande = parametros.TamanioGrande,
                    TamanioMediano = parametros.TamanioMediano,
                    TamanioPequenio = parametros.TamanioPequenio,
                    TamanioToy = parametros.TamanioToy,
                    CantidadPerro = (int)parametros.CantidadPerro
                };

                paseador.PerroAceptado = primerPerroAceptado;
            }
            else
            {
                paseador.PerroAceptado.TamanioGigante = parametros.TamanioGigante;
                paseador.PerroAceptado.TamanioGrande = parametros.TamanioGrande;
                paseador.PerroAceptado.TamanioMediano = parametros.TamanioMediano;
                paseador.PerroAceptado.TamanioPequenio = parametros.TamanioPequenio;
                paseador.PerroAceptado.TamanioToy = parametros.TamanioToy;
                paseador.PerroAceptado.CantidadPerro = (int)parametros.CantidadPerro;
            }
            

            //Tarifa
            if(paseador.Tarifas == null)
            {
                Tarifa primeraTarifa = new Tarifa
                {
                    Basico = (int)parametros.Basico,
                    Juego = (int)parametros.Juego,
                    Social = (int)parametros.Social,
                    FechaRegistro = DateTime.Now,
                    FechaTermino = null
                };

                paseador.Tarifas.Add(primeraTarifa);
            }
            else
            {
                var tarifa = paseador.Tarifas.FirstOrDefault(t => t.FechaTermino == null);
                
                if(tarifa == null)
                {
                    return NotFound(new { mensaje = "No se pudo obtener la última tarifa del paseador" });
                }

                if(!(tarifa.Basico == parametros.Basico && tarifa.Juego == parametros.Juego && tarifa.Social == parametros.Social))
                {
                    Tarifa nuevaTarifa = new Tarifa
                    {
                        Basico = (int)parametros.Basico,
                        Juego = (int)parametros.Juego,
                        Social = (int)parametros.Social,
                        FechaRegistro = DateTime.Now,
                        FechaTermino = null
                    };

                    paseador.Tarifas.Add(nuevaTarifa);

                    tarifa.FechaTermino = DateTime.Now;

                }
            }

            var modificacionParametrosLaborales = await _context.SaveChangesAsync();

            if(modificacionParametrosLaborales <= 0)
            {
                return BadRequest(new { mensaje = "No se pudo editar los parámetros laborales del paseador" });
            }

            return NoContent();

        }
        
        [HttpGet]
        [Route("ObtenerCercanos")]
        [Authorize(Policy ="Tutor")]
        public async Task<IActionResult> ObtenerPaseadoresCercanos(int codigoComuna)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            /*
            
            //obtener coordenadas API
             
            var ubicacion = await _context.Ubicaciones
                .AsNoTracking()
                .FirstOrDefaultAsync(ub => ub.CodigoComuna == ubicacionVM.CodigoComuna && ub.Direccion == ubicacionVM.Direccion);

            if(ubicacion == null)
            {

            }
            */

            var paseadores = await _context.Paseadores
                .Include(p => p.PerroAceptado)
                .Include(p => p.Tarifas)
                .Include(p => p.ExperienciaPaseador)
                .Include(p => p.Paseos)
                .ThenInclude(ps => ps.Calificacion)
                .Include(p => p.Usuario)
                .ThenInclude(u => u.Imagen)
                .Include(p => p.Ubicacion)
                .Where(p => p.Ubicacion.CodigoComuna == codigoComuna && p.Activado && p.Tarifas != null && p.PerroAceptado != null)
                .Select(p => new Paseador
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Apellido = p.Apellido,
                    Descripcion = p.Descripcion,
                    ExperienciaPaseador = new ExperienciaPaseador
                    {
                        Descripcion = p.ExperienciaPaseador.Descripcion
                    },
                    Usuario = new Usuario
                    {
                        Imagen = new Imagen
                        {
                            Path = p.Usuario.Imagen.Path,
                            Url = p.Usuario.Imagen.Url
                        }
                    },
                    PerroAceptado = new PerroAceptado
                    {
                        TamanioGigante = p.PerroAceptado.TamanioGigante,
                        TamanioGrande = p.PerroAceptado.TamanioGrande,
                        TamanioMediano = p.PerroAceptado.TamanioMediano,
                        TamanioPequenio = p.PerroAceptado.TamanioPequenio,
                        TamanioToy = p.PerroAceptado.TamanioToy,
                        CantidadPerro = p.PerroAceptado.CantidadPerro
                    },
                    Tarifas = p.Tarifas,
                    Paseos = p.Paseos

                })
                .AsNoTracking()
                .ToListAsync();

            if(paseadores == null || paseadores.Count() == 0)
            {
                return NotFound(new { mensaje = "No hay paseadores en la comuna" });
            }

            List<PaseadorVM> paseadoresVM = new List<PaseadorVM>();

            foreach (var paseador in paseadores)
            {
                PaseadorVM paseadorVM = new PaseadorVM
                {
                    Id = paseador.Id,
                    Nombre = paseador.Nombre,
                    Apellido = paseador.Apellido,
                    Descripcion = paseador.Descripcion,
                    ExperienciaPaseador = paseador.ExperienciaPaseador,
                    Usuario = paseador.Usuario,
                    PerroAceptado = paseador.PerroAceptado,
                    TarifaActual = paseador.Tarifas.FirstOrDefault(t => t.FechaTermino == null),
                    //Calificacion = paseador.Paseos.Where(ps => ps.Calificado).Average(ps => ps.Calificacion.Valor)
                };

                paseadoresVM.Add(paseadorVM);
            }

            if(paseadoresVM.Count() == 0)
            {
                return BadRequest(new { mensaje = "No se pudo obtener los paseadores de la comuna" });
            }

            return Ok(paseadoresVM);

        }

        [HttpGet]
        [Route("ObtenerPostulantes")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> ObtenerPaseadoresPostulantes()
        {
            var paseadores = await _context.Paseadores
                .Include(p => p.Usuario)
                .Include(p => p.ExperienciaPaseador)
                .Where(p => p.Activado == false && p.ExperienciaPaseador.Id == 1)
                .Select(p => new Paseador
                {
                    Id = p.Id,
                    Rut = p.Rut,
                    Dv = p.Dv,
                    Nombre = p.Nombre,
                    Apellido = p.Apellido,
                    Telefono = p.Telefono,
                    Usuario = new Usuario
                    {
                        Email = p.Usuario.Email
                    },
                    ExperienciaPaseador =
                    {
                        Id = p.ExperienciaPaseador.Id,
                        Descripcion = p.ExperienciaPaseador.Descripcion
                    }
                })
                .AsNoTracking()
                .ToListAsync();

            if(paseadores == null || paseadores.Count() == 0)
            {
                return NotFound(new { mensaje = "No hay paseadores postulantes" });
            }

            return Ok(paseadores);

        }

        [HttpPut]
        [Route("Activar/{id:int}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> ActivarPaseadorPostulante(int id, int codigoExperiencia)
        {
            var experienciaPaseador = await _context.ExperienciaPaseadores.FindAsync(codigoExperiencia);

            if(experienciaPaseador == null)
            {
                return NotFound(new { mensaje = "No se pudo obtener la experiencia para el paseador" });
            }

            if (experienciaPaseador.Id == 1)
            {
                return Ok(new { mensaje = "No se activó la cuenta del paseador ya que no se le asignó la experiencia necesaria para realizar paseos" });
            }

            var paseador = await _context.Paseadores.FindAsync(id);

            if(paseador == null)
            {
                return NotFound(new { mensaje = "No se pudo obtener al paseador" });
            }                       

            paseador.ExperienciaPaseador = experienciaPaseador;
            paseador.Activado = true;

            var modificacionExperiencia = await _context.SaveChangesAsync();

            if(modificacionExperiencia <= 0)
            {
                return BadRequest(new { mensaje = "No se pudo cambiar la experiencia para el paseador" });
            }

            return NoContent();

        }



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

