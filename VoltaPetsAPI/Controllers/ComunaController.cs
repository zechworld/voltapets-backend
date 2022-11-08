using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using VoltaPetsAPI.Data;
using VoltaPetsAPI.Models;

namespace VoltaPetsAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ComunaController : ControllerBase
    {
        private readonly VoltaPetsContext _context;

        public ComunaController(VoltaPetsContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerComunas()
        {
            var comunas = await _context.Comunas.ToListAsync();

            if (comunas.Any())
            {
                return Ok(comunas);
            }
            else
            {
                return BadRequest("No existen comunas en la base de datos");
            }
        }

        [HttpGet("{codigoRegion}")]
        public async Task<IActionResult> ObtenerComunasRegion(int codigoRegion)
        {
            var comunas = await _context.Comunas
                .Include(c => c.Provincia)
                .ThenInclude(p => p.Region)
                .ThenInclude(r => r.CodigoRegion == codigoRegion)
                .Select(c => new Comuna()
                {
                    CodigoComuna=c.CodigoComuna,
                    Descripcion=c.Descripcion,
                })
                .ToListAsync();

            if (comunas.Any())
            {
                return Ok(comunas);
            }
            else
            {
                return BadRequest("No existen comunas en la base de datos");
            }

        }

    }
}
