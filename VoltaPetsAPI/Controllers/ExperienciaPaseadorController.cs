using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using VoltaPetsAPI.Data;

namespace VoltaPetsAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ExperienciaPaseadorController : ControllerBase
    {
        private readonly VoltaPetsContext _context;

        public ExperienciaPaseadorController(VoltaPetsContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("Obtener")]
        public async Task<IActionResult> ObtenerExperienciasPaseador()
        {
            var experienciasPaseador = await _context.ExperienciaPaseadores
                .AsNoTracking()
                .ToListAsync();

            return Ok(experienciasPaseador);

        }








    }
}
