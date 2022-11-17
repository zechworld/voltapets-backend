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
    public class RegionController : ControllerBase
    {
        private readonly VoltaPetsContext _context;

        public RegionController(VoltaPetsContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerRegiones()
        {
            var regiones = await _context.Regiones
                .AsNoTracking()
                .ToListAsync();

            if (regiones.Any())
            {
                return Ok(regiones);
            }
            else
            {
                return BadRequest("No existen regiones en la base de datos");
            }

        }


    }
}
