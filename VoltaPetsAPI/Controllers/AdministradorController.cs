using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VoltaPetsAPI.Data;

namespace VoltaPetsAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AdministradorController : ControllerBase
    {
        private readonly VoltaPetsContext _context;

        public AdministradorController(VoltaPetsContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("AllUsuarios")]
        [AllowAnonymous]
        public async Task<IActionResult> ObtenerUsuarios()
        {
            var usuarios = await _context.Usuarios
                .Include(user => user.Rol)
                .Include(user => user.Paseador)
                .Include(user => user.Tutor)
                .ToListAsync();

            if (usuarios == null)
            {
                return NotFound(new { mensaje = "No hay usuarios registrados en la aplicación" });
            }

            return Ok(usuarios);

        }

        [HttpGet]
        [Route("AllMascotas")]
        [AllowAnonymous]
        public async Task<IActionResult> ObtenerMascotas()
        {
            var mascotas = await _context.Mascotas
                .Include(mascota => mascota.EstadoMascota)
                .Include(mascota => mascota.Tutor)
                .Include(mascota => mascota.GrupoEtario)
                .Include(mascota => mascota.VacunaMascotas)
                .ThenInclude(vacuna => vacuna.Vacuna)
                .ToListAsync();

            if (mascotas == null)
            {
                return NotFound(new { mensaje = "No hay mascotas registradas en la aplicación" });
            }

            return Ok(mascotas);
        }


        [HttpGet]
        [Route("AllTutores")]
        [AllowAnonymous]
        public async Task<IActionResult> ObtenerTutores()
        {
            var tutores = await _context.Tutores
                .Include(tutor => tutor.Usuario)
                .ThenInclude(user => user.Imagen)
                .ToListAsync();

            if (tutores == null)
            {
                return NotFound(new { mensaje = "No se han encontrado tutores registrados en la aplicación" });
            }

            return Ok(tutores);
        }

        [HttpGet]
        [Route("AllPaseadores")]
        [AllowAnonymous]
        public async Task<IActionResult> ObtenerPaseadores()
        {
            var paseadores = await _context.Paseadores
                .Include(paseador => paseador.Usuario)
                .ThenInclude(user => user.Imagen)
                .ToListAsync();

            if (paseadores == null)
            {
                return NotFound(new { mensaje = "No hay paseadores registrados en la aplicación" });
            }

            return Ok(paseadores);
        }

    }

}
