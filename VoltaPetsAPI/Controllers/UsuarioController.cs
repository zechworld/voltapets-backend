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
using Microsoft.AspNetCore.WebUtilities;

namespace VoltaPetsAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly Cloudinary _cloudinary;
        private readonly VoltaPetsContext _context;
        private readonly IConfiguration _config;
        private IEmailService _mailService;

        public UsuarioController(VoltaPetsContext context, IConfiguration config, Cloudinary cloudinary, IEmailService mailService)
        {
            _context = context;
            _config = config;
            _cloudinary = cloudinary;
            _mailService = mailService;

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
                .FirstOrDefaultAsync(usr => usr.Id == codigoUsuario);

            if (usuario != null)
            {
                // Si es Administrador
                if (usuario.CodigoRol == 1)
                {
                    var admin = await _context.Administradores
                        .FirstOrDefaultAsync(admin => admin.CodigoUsuario == usuario.Id);

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
                        .FirstOrDefaultAsync(paseador => paseador.CodigoUsuario == usuario.Id);

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
                if (usuario.CodigoRol == 3)
                {
                    var tutor = await _context.Tutores
                        .FirstOrDefaultAsync(tutor => tutor.CodigoUsuario == usuario.Id);

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

                return NotFound(new { mensaje = "No se ha encontrado al usuario" });
            }

            return BadRequest(new { mensaje = "Ha ocurrido un error" });
        }

        [HttpPut]
        [Route("RegistrarImagen")]
        [AllowAnonymous]
        public async Task<IActionResult> RegistrarImagenPerfil(UserImagen img)
        {
            if (!ModelState.IsValid)
            {
                ImagenCloudinary.EliminarImagenHosting(_cloudinary, img.ToImagen());
                return BadRequest(ModelState);
            }

            var usuario = await _context.Usuarios.FindAsync(img.CodigoUsuario);

            if (usuario == null)
            {
                ImagenCloudinary.EliminarImagenHosting(_cloudinary, img.ToImagen());
                return NotFound(new { mensaje = "Usuario no encontrado" });
            }


            var imagen = new Imagen();
            imagen.Url = img.Url;
            imagen.Path = img.Path;
            imagen.Public_Id = img.Public_Id;


            _context.Imagenes.Add(imagen);   //TODO: Eliminar

            usuario.CodigoImagen = imagen.Id; //TODO: Eliminar
            usuario.Imagen = imagen;

            var modificacionImagen = await _context.SaveChangesAsync();

            if (modificacionImagen <= 0)
            {
                ImagenCloudinary.EliminarImagenHosting(_cloudinary, img.ToImagen());
                return BadRequest(new { mensaje = "No se pudo cambiar la imagen de perfil" });
            }

            return NoContent();
        }

        [HttpPut]
        [Route("CambiarImagen")]
        [Authorize(Policy = "Usuario")]
        public async Task<IActionResult> CambiarImagenPerfil(ImagenVM imagen)
        {
            if (!ModelState.IsValid)
            {
                ImagenCloudinary.EliminarImagenHosting(_cloudinary, imagen.ToImagen());
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
                ImagenCloudinary.EliminarImagenHosting(_cloudinary, imagen.ToImagen());
                return BadRequest(new { mensaje = "Error en obtener el usuario actual" });
            }

            var usuario = await _context.Usuarios
                .Include(user => user.Imagen)
                .FirstOrDefaultAsync(user => user.Id == codigoUsuario);

            if (usuario == null)
            {
                ImagenCloudinary.EliminarImagenHosting(_cloudinary, imagen.ToImagen());
                return NotFound(new { mensaje = "Usuario no encontrado" });
            }

            var imagenAnteriorPublicId = usuario.Imagen.Public_Id;

            usuario.Imagen.Public_Id = imagen.Public_Id;
            usuario.Imagen.Url = imagen.Url;
            usuario.Imagen.Path = imagen.Path;
            var modificacionImagen = await _context.SaveChangesAsync();

            if (modificacionImagen <= 0)
            {
                ImagenCloudinary.EliminarImagenHosting(_cloudinary, imagen.ToImagen());
                return BadRequest(new { mensaje = "No se pudo cambiar la imagen de perfil" });
            }
            else
            {
                if (imagenAnteriorPublicId != _config["Cloudinary:DefaultPublicID"])
                {
                    imagen.Public_Id = imagenAnteriorPublicId;
                    ImagenCloudinary.EliminarImagenHosting(_cloudinary, imagen.ToImagen());

                }

                return NoContent();
            }

        }

        [HttpPost]
        [Route("ForgetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> RecuperarPwd([FromBody] ForgetPasswordVM pwd)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(pwd.Email))
                {
                    return NotFound(new { mensaje = $"El correo {pwd.Email} no tiene un usuario asociado" });
                }

                var user = await _context.Usuarios
                    .Include(user => user.Tutor)
                    .Include(user => user.Paseador)
                    .FirstOrDefaultAsync(user => user.Email.Equals(pwd.Email));

                if (user != null)
                {
                    var nombre = "";

                    if (user.CodigoRol == 2)
                    {
                        nombre = user.Paseador.Nombre;
                    }
                    else
                    {
                        nombre = user.Tutor.Nombre;
                    }

                    var token = BuildToken(user);
                    var encodedToken = Encoding.UTF8.GetBytes(token);
                    var validateToken = WebEncoders.Base64UrlEncode(encodedToken);

                    var url = $"{_config["AppUrl"]}/password/reset?email={pwd.Email}&token={validateToken}";
                    var templateId = _config["SendGrid:PwdRecoveryTemplate"];

                    EmailTemplateUser data = new EmailTemplateUser
                    {
                        Nombre = nombre,
                        Url = url
                    };

                    await _mailService.SendEmailAsync(pwd.Email, templateId, data);
                    return Ok();
                }

                return BadRequest(new { mensaje = $"El correo {pwd.Email} no tiene un usuario asociado" });
            }
            return BadRequest(new { mensaje = "Ha ocurrido un error con la solicitud" });
        }

        [HttpPost]
        [Route("ResetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPwd([FromBody] ResetPasswordVM pwd)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Usuarios.FirstOrDefaultAsync(user => user.Email.Equals(pwd.Email));

                if (user == null)
                {
                    NotFound(new { mensaje = "No existe un usuario asociado a este correo" });
                }

                if (pwd.Password != pwd.ConfirmPassword)
                {
                    BadRequest(new { mensaje = "Las contraseñas no coinciden" });
                }

                var decodedToken = WebEncoders.Base64UrlDecode(pwd.Token);
                string tokenNormal = Encoding.UTF8.GetString(decodedToken);
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(tokenNormal);
                var decodedEmail = jwtToken.Claims.ToList()[0].Value;


                if (user.Email == decodedEmail)
                {
                    user.Password = Encriptacion.GetSHA256(pwd.Password);
                    var result = await _context.SaveChangesAsync();
                    return NoContent();
                }

            }
            return BadRequest(new { mensaje = "Ha ocurrido un error al restablecer las contraseñas" });
        }

        private string BuildToken(Usuario usuario)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
                new Claim(JwtRegisteredClaimNames.Sid, usuario.Id.ToString()),
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
