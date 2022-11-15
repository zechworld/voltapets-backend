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
using VoltaPetsAPI.Models.ViewModels;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;

namespace VoltaPetsAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly Cloudinary _cloudinary;
        private readonly VoltaPetsContext _context;
        private readonly IConfiguration _config;

        public UsuarioController(VoltaPetsContext context, IConfiguration config, Cloudinary cloudinary)
        {
            _context = context;
            _config = config;
            _cloudinary = cloudinary;
            
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

            return Ok(new { token = token, codigoRol = usuario.CodigoRol });

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
                .Include(u => u.Rol)
                .Include(u => u.Imagen)
                .FirstOrDefaultAsync(usr => usr.CodigoUsuario == codigoUsuario);

            if (usuario != null)
            {
                // Si es Administrador
                if (usuario.CodigoRol == 1)
                {
                    var admin = await _context.Administradores
                        .FirstOrDefaultAsync(admin => admin.CodigoUsuario == usuario.CodigoUsuario);

                    return Ok(new
                    {
                        id = admin.CodigoUsuario,
                        rol = usuario.Rol.Descripcion,
                        nombre = admin.Nombre,
                        apellido = admin.Apellido,
                        email = usuario.Email
                    });
                }

                // Si es Paseador
                if (usuario.CodigoRol == 2)
                {
                    var paseador = await _context.Paseadores
                        .FirstOrDefaultAsync(paseador => paseador.CodigoUsuario == usuario.CodigoUsuario);

                    return Ok(new
                    {
                        id = paseador.CodigoUsuario,
                        rol = usuario.Rol.Descripcion,
                        nombre = paseador.Nombre,
                        apellido = paseador.Apellido,
                        email = usuario.Email,
                        imagen = usuario.Imagen
                    });
                        
                }

                // Si es Tutor
                if (usuario.CodigoRol == 3) {
                    var tutor = await _context.Tutores
                        .FirstOrDefaultAsync(tutor => tutor.CodigoUsuario == usuario.CodigoUsuario);

                    return Ok(new
                    {
                        id = tutor.CodigoUsuario,
                        rol = usuario.Rol.Descripcion,
                        nombre = tutor.Nombre,
                        apellido = tutor.Apellido,
                        email = usuario.Email,
                        imagen = usuario.Imagen
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
        public async Task<IActionResult> CambiarImagenPerfil(ImagenVM imagen)
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

            var usuario = await _context.Usuarios
                .Include(user => user.Imagen)
                .FirstOrDefaultAsync(user => user.CodigoUsuario == codigoUsuario);

            if (usuario == null)
            {
                return NotFound(new { mensaje = "Usuario no encontrado" });
            }


            usuario.Imagen.Url = imagen.Url;
            usuario.Imagen.Path = imagen.Path;
            await _context.SaveChangesAsync();

            //var deletionParams = new DeletionParams(imagen.Public_id);
            //var resultadoEliminacion = _cloudinary.Destroy(deletionParams);

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
