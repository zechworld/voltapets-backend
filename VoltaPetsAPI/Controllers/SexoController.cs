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
    public class SexoController : ControllerBase
    {
        private readonly VoltaPetsContext _context;

        public SexoController(VoltaPetsContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("Obtener/Todo")]
        [AllowAnonymous]
        public async Task<IActionResult> ObtenerSexos()
        {
            var sexos = await _context.Sexos
                .AsNoTracking()
                .ToListAsync();

            if (!sexos.Any())
            {
                return BadRequest(new { mensaje = "No existen Sexos en la base de datos" });
            }

            return Ok(sexos);

        }

    }
}
