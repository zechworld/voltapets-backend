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
        }




    }
}
