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

        [Route("Login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserLogin userLogin)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var usuario = await _context.Usuarios.Where(u => u.Email == userLogin.Email).FirstOrDefaultAsync();
            if (usuario == null)
            {
                return NotFound("El Usuario no existe");
            }

            if(usuario.Password != Encriptacion.GetSHA256(userLogin.Password))
            {
                return Unauthorized("Contraseña incorrecta");
            }

            var token = BuildToken(usuario);

            usuario.Token = token;

            await _context.SaveChangesAsync();

            return Ok(token);
                        
        }

        [HttpPut]
        public async Task<IActionResult> AsignarImagenPerfil(Imagen imagen)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            if(await _context.Imagenes.Where(img => img.Url.Equals(imagen.Url) && img.Path.Equals(imagen.Path)).AnyAsync())
            {
                return BadRequest("La imagen ya existe en la base de datos");
            }

            
            var claims = (ClaimsIdentity)User.Identity;
            var codUser = claims.FindFirst(JwtRegisteredClaimNames.Sid).Value;
            int codigoUsuario;

            if(int.TryParse(codUser, out int id))
            {
                codigoUsuario = id;
            }
            else
            {
                codigoUsuario = 0;
            }

            var usuario = await _context.Usuarios.FindAsync(codigoUsuario);
            
            if(usuario == null)
            {
                return NotFound("Usuario no encontrado");
            }

            usuario.CodigoImagen = imagen.CodigoImagen;
            await _context.SaveChangesAsync();



            return Ok();
        }

        [HttpPut]





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
