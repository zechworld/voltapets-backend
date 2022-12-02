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
    public class RazaController : ControllerBase
    {
        private readonly VoltaPetsContext _context;

        public RazaController(VoltaPetsContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("Obtener")]
        [AllowAnonymous]
        public async Task<IActionResult> ObtenerRazas()
        {
            var razas = await _context.Razas
                .AsNoTracking()
                .ToListAsync();

            if (!razas.Any())
            {
                return BadRequest(new { mensaje = "No existen Razas en la base de datos" });
            }

            return Ok(razas);

        }

    }
}
