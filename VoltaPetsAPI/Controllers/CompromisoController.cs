using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using VoltaPetsAPI.Data;
using VoltaPetsAPI.Models;
using VoltaPetsAPI.Models.ViewModels;

namespace VoltaPetsAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CompromisoController : ControllerBase
    {
        private readonly VoltaPetsContext _context;

        public CompromisoController(VoltaPetsContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("Registrar")]
        [Authorize(Policy = "Tutor")]
        public async Task<IActionResult> RegistrarCompromiso(CompromisoVM compromisoVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //validar fecha compromiso
            if (compromisoVM.FechaCompromiso <= DateTime.Now.Date)
            {
                return BadRequest(new { mensaje = "La fecha del compromiso no puede ser anterior o igual a la fecha actual" });
            }

            Compromiso compromiso = new Compromiso
            {
                CodigoMascota = (int)compromisoVM.CodigoMascota,
                Titulo = compromisoVM.Titulo,
                FechaCompromiso = (DateTime)compromisoVM.FechaCompromiso
            };

            _context.Compromisos.Add(compromiso);
            var registroCompromiso = await _context.SaveChangesAsync();

            if (registroCompromiso <= 0)
            {
                return BadRequest(new { mensaje = "No se pudo registrar el compromiso" });
            }

            return Ok(new { mensaje = "Se registro el compromiso con éxito" });

        }

        [HttpGet]
        [Route("Obtener/{codigoMascota:int}")]
        [Authorize(Policy = "Tutor")]
        public async Task<IActionResult> ObtenerCompromisosMascota(int codigoMascota)
        {
            var compromisosMascota = await _context.Compromisos
                .Where(cm => cm.CodigoMascota == codigoMascota)
                .OrderBy(cm => cm.FechaCompromiso)
                .AsNoTracking()
                .ToListAsync();

            return Ok(compromisosMascota);

        }














    }
}
