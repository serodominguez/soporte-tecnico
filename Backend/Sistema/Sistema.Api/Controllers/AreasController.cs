using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sistema.Api.Models;
using Sistema.Core.Entities;
using Sistema.Infrastructure.Data;

namespace Sistema.Api.Controllers
{
    [Authorize(Roles = "Administrador,Jefe de Sistemas,Soporte Fabrica,Soporte Tienda")]
    [Route("api/[controller]")]
    [ApiController]
    public class AreasController : ControllerBase
    {
        private readonly DbContextSistema _context;

        public AreasController(DbContextSistema context)
        {
            _context = context;
        }

        //GET: api/Areas/Listar
        [HttpGet("[action]")]
        public async Task<IEnumerable<AreasModel>> Listar()
        {
            var area = await _context.Area.OrderByDescending(a => a.IdArea).ToListAsync();
            return area.Select(a => new AreasModel
            {
                IdArea = a.IdArea,
                Area = a.Area,
                Tipo = a.Tipo,
                Estado = a.Estado
            });
        }

        // GET: api/Areas/SeleccionarInternos
        [HttpGet("[action]")]
        public async Task<IEnumerable<AreasModel>> SeleccionarInternos()
        {
            var area = await _context.Area.Where(a => a.Tipo == "INTERNOS" && a.Estado == "Activo").OrderBy(a => a.Area).ToListAsync();
            return area.Select(a => new AreasModel
            {
                IdArea = a.IdArea,
                Area = a.Area
            });
        }

        // GET: api/Areas/SeleccionarExternos
        [HttpGet("[action]")]
        public async Task<IEnumerable<AreasModel>> SeleccionarExternos()
        {
            var area = await _context.Area.Where(a => a.Tipo == "EXTERNOS" && a.Estado == "Activo").OrderBy(a => a.Area).ToListAsync();
            return area.Select(a => new AreasModel
            {
                IdArea = a.IdArea,
                Area = a.Area
            });
        }

        // PUT: api/Areas/Actualizar
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("[action]")]
        public async Task<IActionResult> Actualizar([FromBody] AreasModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (await _context.Area.AnyAsync(a => a.IdArea != model.IdArea && a.Area == model.Area))
            {
                return BadRequest();
            }

            if (model.IdArea < 0)
            {
                return BadRequest();
            }

            var area = await _context.Area.FirstOrDefaultAsync(a => a.IdArea == model.IdArea);

            if (area == null)
            {
                return NotFound();
            }

            area.Area = model.Area;
            area.Tipo = model.Tipo;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }

            return Ok();
        }

        // POST: api/Areas/Crear
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("[action]")]
        public async Task<ActionResult> Crear([FromBody] AreasModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (await _context.Area.AnyAsync(a => a.IdArea != model.IdArea && a.Area == model.Area))
            {
                return BadRequest();
            }

            Areas area = new Areas
            {
                Area = model.Area,
                Tipo = model.Tipo,
                Estado = model.Estado
            };

            _context.Area.Add(area);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

            return Ok();
        }

        // PUT: api/Areas/Desactivar/1
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Desactivar([FromRoute] int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var area = await _context.Area.FirstOrDefaultAsync(a => a.IdArea == id);

            if (area == null)
            {
                return NotFound();
            }

            area.Estado = "Inactivo";

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }

            return Ok();
        }

        // PUT: api/Areas/Activar/1
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Activar([FromRoute] int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var area = await _context.Area.FirstOrDefaultAsync(a => a.IdArea == id);

            if (area == null)
            {
                return NotFound();
            }

            area.Estado = "Activo";

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
