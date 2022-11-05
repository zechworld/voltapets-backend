using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using VoltaPetsAPI.Data;
using VoltaPetsAPI.Helpers;
using VoltaPetsAPI.Models;

namespace VoltaPetsAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly VoltaPetsContext _context;

        public UsuarioController(VoltaPetsContext context)
        {
            _context = context;
        }

        [Route("Registar")]
        [HttpPost]
        public async Task<IActionResult> RegistrarUsuario([FromBody] Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(await _context.Usuarios.Where(u => u.Email == usuario.Email).AnyAsync())
            {
                return BadRequest("El Correo ya tiene un usuario asociado");
            }

            usuario.Password = Encriptacion.GetSHA256(usuario.Password);
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return NoContent();

        }






    }
}
