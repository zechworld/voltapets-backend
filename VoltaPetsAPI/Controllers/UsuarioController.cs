using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using VoltaPetsAPI.Data;
using VoltaPetsAPI.Helpers;
using VoltaPetsAPI.Models;
using VoltaPetsAPI.Models.User;
using Microsoft.AspNetCore.Authorization;

namespace VoltaPetsAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly VoltaPetsContext _context;
        private readonly IConfiguration _config;

        public UsuarioController(VoltaPetsContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] UserLogin userLogin)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var usuario = await _context.Usuarios.Where(u => u.Email == userLogin.Email).FirstOrDefaultAsync();
            if (usuario == null)
            {
                return NotFound(new { mensaje = "El Usuario no existe" });
            }

            if (!usuario.Password.Equals(Encriptacion.GetSHA256(userLogin.Password)))
            {
                return Unauthorized(new { mensaje = "Contraseña incorrecta" });
            }

            var token = BuildToken(usuario);

            usuario.Token = token;

            await _context.SaveChangesAsync();

            return Ok(new { token = token });

        }

        [Route("getUsuarioToken")]
        [HttpGet]
        public async Task<IActionResult> ObtenerUsuarioToken()
        {
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

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(usr => usr.CodigoUsuario == codigoUsuario);

            if (usuario != null)
            {
                // Si es Administrador
                if (usuario.CodigoRol == 1)
                {
                    var admin = await _context.Administradores
                        .Include(admin => admin.Usuario)
                        .FirstOrDefaultAsync(admin => admin.CodigoUsuario == usuario.CodigoUsuario);

                    return Ok(new
                    {
                        id = admin.CodigoUsuario,
                        rol = admin.Usuario.Rol.Descripcion,
                        nombre = admin.Nombre,
                        apellido = admin.Apellido,
                        email = admin.Usuario.Email
                    });
                }

                // Si es Paseador
                if (usuario.CodigoRol == 2)
                {
                    var paseador = await _context.Paseadores
                        .Include(paseador => paseador.Usuario)
                        .ThenInclude(usuario => usuario.Imagen)
                        .Include(paseador => paseador.Usuario.Rol)
                        .FirstOrDefaultAsync(paseador => paseador.CodigoUsuario == usuario.CodigoUsuario);

                    return Ok(new
                    {
                        id = paseador.CodigoUsuario,
                        rol = paseador.Usuario.Rol.Descripcion,
                        nombre = paseador.Nombre,
                        apellido = paseador.Apellido,
                        email = paseador.Usuario.Email,
                        imagen = paseador.Usuario.Imagen
                    });
                        
                }

                // Si es Tutor
                if (usuario.CodigoRol == 3) {
                    var tutor = await _context.Tutores
                        .Include(tutor => tutor.Usuario)
                        .ThenInclude(usuario => usuario.Imagen)
                        .Include(tutor => tutor.Usuario.Rol)
                        .FirstOrDefaultAsync(tutor => tutor.CodigoUsuario == usuario.CodigoUsuario);

                    return Ok(new
                    {
                        id = tutor.CodigoUsuario,
                        rol = tutor.Usuario.Rol.Descripcion,
                        nombre = tutor.Nombre,
                        apellido = tutor.Apellido,
                        email = tutor.Usuario.Email,
                        imagen = tutor.Usuario.Imagen
                    });
                }

                return NotFound(new { mensaje = "No se ha encontrado al usuario"});
            }

            return BadRequest(new {mensaje = "Ha ocurrido un error"});
        }

        [HttpPut]
        [Route("RegistrarImagen")]
        [AllowAnonymous]               
        public async Task<IActionResult> RegistrarImagenPerfil(UserImagen img)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var usuario = await _context.Usuarios.FindAsync(img.CodigoUsuario);

            if (usuario == null)
            {
                return NotFound(new { mensaje = "Usuario no encontrado" });
            }

            var imagen = new Imagen();
            imagen.Url = img.Url;
            imagen.Path = img.Path;

            _context.Imagenes.Add(imagen);

            usuario.CodigoImagen = imagen.CodigoImagen;
            usuario.Imagen = imagen;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut]
        [Route("CambiarImagen")]
        [Authorize(Policy = "Usuario")]        
        public async Task<IActionResult> CambiarImagenPerfil(Imagen imagen)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var claims = (ClaimsIdentity)User.Identity;
            var codUser = claims.FindFirst(JwtRegisteredClaimNames.Sid).Value;
            int codigoUsuario;

            if (int.TryParse(codUser, out int id))
            {
                codigoUsuario = id;
            }
            else
            {
                return BadRequest(new { mensaje = "Error en obtener el usuario actual" });
            }

            var usuario = await _context.Usuarios.FindAsync(codigoUsuario);

            if (usuario == null)
            {
                return NotFound(new { mensaje = "Usuario no encontrado" });
            }

            usuario.Imagen = imagen;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private string BuildToken(Usuario usuario)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
                new Claim(JwtRegisteredClaimNames.Sid, usuario.CodigoUsuario.ToString()),
                new Claim("Rol", usuario.CodigoRol.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var expiration = DateTime.UtcNow.AddDays(7);

            JwtSecurityToken token = new JwtSecurityToken(
                //issuer: _config["Jwt:Issuer"],
                //audience: _config["Jwt:Audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
