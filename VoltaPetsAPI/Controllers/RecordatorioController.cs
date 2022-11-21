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
    public class RecordatorioController : ControllerBase
    {
        private readonly VoltaPetsContext _context;

        public RecordatorioController(VoltaPetsContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("Registrar")]
        [Authorize(Policy = "Tutor")]
        public async Task<IActionResult> RegistrarRecordatorio(RecordatorioVM recordatorioVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Recordatorio recordatorio = new Recordatorio
            {
                CodigoMascota = (int)recordatorioVM.CodigoMascota,
                Titulo = recordatorioVM.Titulo,
                Descripcion = recordatorioVM.Descripcion,
                FechaPublicacion = DateTime.Now
            };

            _context.Recordatorios.Add(recordatorio);
            var registroRecordatorio = await _context.SaveChangesAsync();

            if(registroRecordatorio <= 0)
            {
                return BadRequest(new { mensaje = "No se pudo registrar el recordatorio" });
            }

            return Ok(new { mensaje = "Recordatorio registrado con éxito" });

        }

        [HttpGet]
        [Route("Obtener/{codigoMascota:int}")]
        [Authorize(Policy = "Tutor")]
        public async Task<IActionResult> ObtenerRecordatoriosMascota(int codigoMascota)
        {
            var recordatoriosMascota = await _context.Recordatorios
                .Where(rc => rc.CodigoMascota == codigoMascota)
                .OrderByDescending(rc => rc.FechaPublicacion)
                .AsNoTracking()
                .ToListAsync();

            return Ok(recordatoriosMascota);

            }

            }
            }

