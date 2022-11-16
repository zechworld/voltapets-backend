using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VoltaPetsAPI.Data;
using VoltaPetsAPI.Models;

namespace VoltaPetsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MascotaController : ControllerBase
    {
        private readonly VoltaPetsContext _context;

        public MascotaController(VoltaPetsContext context)
        {
            _context = context;
        }
        /*
        [HttpPost]
        [Route("Registrar")]
        [Authorize(Policy = "Tutor")]
        
        public  async Task<IActionResult> RegistrarMascota(Mascota mascota)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }







        }

        */















    }
}
