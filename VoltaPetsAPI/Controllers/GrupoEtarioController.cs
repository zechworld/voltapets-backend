using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using VoltaPetsAPI.Data;

namespace VoltaPetsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GrupoEtarioController : ControllerBase
    {
        private readonly VoltaPetsContext _context;

        public GrupoEtarioController(VoltaPetsContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("Obtener/Todo")]
        [AllowAnonymous]
        public async Task<IActionResult> ObtenerGruposEtarios()
        {
            var gruposEtarios = await _context.GrupoEtarios
                .AsNoTracking()
                .ToListAsync();

            if (!gruposEtarios.Any())
            {
                return BadRequest(new { mensaje = "No existen Grupos Etarios en la base de datos" });
            }

            return Ok(gruposEtarios);

        }

    }
}
