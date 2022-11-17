using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using VoltaPetsAPI.Data;

namespace VoltaPetsAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TamanioController : ControllerBase
    {
        private readonly VoltaPetsContext _context;

        public TamanioController(VoltaPetsContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("Obtener")]
        [AllowAnonymous]
        public async Task<IActionResult> ObtenerTamanios()
        {
            var tamanios = await _context.Tamanios
                .AsNoTracking()
                .ToListAsync();

            if (!tamanios.Any())
            {
                return BadRequest(new { mensaje = "No existen Tamaños en la base de datos" });
            }

            return Ok(tamanios);

        }

    }
}
