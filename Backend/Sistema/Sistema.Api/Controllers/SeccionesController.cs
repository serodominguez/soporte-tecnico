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
    public class SeccionesController : ControllerBase
    {
        private readonly DbContextSistema _context;

        public SeccionesController(DbContextSistema context)
        {
            _context = context;
        }

        //GET: api/Secciones/ListarInternos
        [HttpGet("[action]")]
        public async Task<IEnumerable<SeccionesModel>> ListarInternos()
        {
            var seccion = await _context.Seccion
                .Include(s => s.areas)
                .Where(a => a.areas.Tipo == "INTERNOS")
                .OrderByDescending(s => s.IdSeccion)
                .ToListAsync();
            return seccion.Select(s => new SeccionesModel
            {
                IdSeccion = s.IdSeccion,
                IdArea = s.IdArea,
                Area = s.areas.Area,
                Seccion = s.Seccion,
                Direccion = s.Direccion,
                PkSeccion = Convert.ToInt32(s.PkSeccion),
                Estado = s.Estado
            });
        }

        //GET: api/Secciones/ListarExternos
        [HttpGet("[action]")]
        public async Task<IEnumerable<SeccionesModel>> ListarExternos()
        {
            var seccion = await _context.Seccion
                .Include(s => s.areas)
                .Where(a => a.areas.Tipo == "EXTERNOS")
                .OrderByDescending(s => s.IdSeccion)
                .ToListAsync();
            return seccion.Select(s => new SeccionesModel
            {
                IdSeccion = s.IdSeccion,
                IdArea = s.IdArea,
                Area = s.areas.Area,
                Seccion = s.Seccion,
                Direccion = s.Direccion,
                Ciudad = s.Ciudad,
                PkTienda = Convert.ToInt32(s.PkTienda),
                Estado = s.Estado
            });
        }

        // GET: api/Secciones/SeleccionarSeccion
        [HttpGet("[action]")]
        public async Task<IEnumerable<SeccionesModel>> SeleccionarSeccion()
        {
            var seccion = await _context.Seccion
                .Where(s => s.IdArea == 1 && s.Estado == "Activo")
                .OrderBy(s => s.Seccion)
                .ToListAsync();
            return seccion.Select(s => new SeccionesModel
            {
                IdSeccion = s.IdSeccion,
                Seccion = s.Seccion
            });
        }

        // GET: api/Secciones/SeleccionarTiendas
        [HttpGet("[action]")]
        public async Task<IEnumerable<SeccionesModel>> SeleccionarTiendas()
        {
            var seccion = await _context.Seccion
                .Where(s => s.IdArea > 1 && s.Estado == "Activo")
                .OrderBy(s => s.Seccion)
                .ToListAsync();
            return seccion.Select(s => new SeccionesModel
            {
                IdSeccion = s.IdSeccion,
                Seccion = s.Seccion
            });
        }

        // PUT: api/Secciones/ActualizarInternos
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("[action]")]
        public async Task<IActionResult> ActualizarInternos([FromBody] SeccionesModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model.IdSeccion < 0)
            {
                return BadRequest();
            }

            if (await _context.Seccion.AnyAsync(s => s.IdSeccion != model.IdSeccion && s.PkSeccion == model.PkSeccion))
            {
                return BadRequest();
            }

            var seccion = await _context.Seccion.FirstOrDefaultAsync(s => s.IdSeccion == model.IdSeccion);

            if (seccion == null)
            {
                return NotFound();
            }

            seccion.IdArea = model.IdArea;
            seccion.Seccion = model.Seccion;
            seccion.Direccion = model.Direccion;
            seccion.PkSeccion = model.PkSeccion;

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

        // PUT: api/Secciones/ActualizarExternos
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("[action]")]
        public async Task<IActionResult> ActualizarExternos([FromBody] SeccionesModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model.IdSeccion < 0)
            {
                return BadRequest();
            }

            if (await _context.Seccion.AnyAsync(s => s.IdSeccion != model.IdSeccion && s.PkTienda == model.PkTienda))
            {
                return BadRequest();
            }

            var seccion = await _context.Seccion.FirstOrDefaultAsync(s => s.IdSeccion == model.IdSeccion);

            if (seccion == null)
            {
                return NotFound();
            }

            seccion.IdArea = model.IdArea;
            seccion.Seccion = model.Seccion;
            seccion.Direccion = model.Direccion;
            seccion.Ciudad = model.Ciudad;
            seccion.PkTienda = model.PkTienda;

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

        // POST: api/Secciones/CrearInternos
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("[action]")]
        public async Task<ActionResult> CrearInternos([FromBody] SeccionesModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (await _context.Seccion.AnyAsync(s => s.IdSeccion != model.IdSeccion && s.PkSeccion == model.PkSeccion))
            {
                return BadRequest();
            }

            Secciones seccion = new Secciones
            {
                IdArea = model.IdArea,
                Seccion = model.Seccion,
                Direccion = model.Direccion,
                PkSeccion = model.PkSeccion,
                Estado = model.Estado
            };

            _context.Seccion.Add(seccion);
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

        // POST: api/Secciones/CrearExternos
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("[action]")]
        public async Task<ActionResult> CrearExternos([FromBody] SeccionesModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (await _context.Seccion.AnyAsync(s => s.IdSeccion != model.IdSeccion && s.PkTienda == model.PkTienda))
            {
                return BadRequest();
            }

            Secciones seccion = new Secciones
            {
                IdArea = model.IdArea,
                Seccion = model.Seccion,
                Direccion = model.Direccion,
                PkTienda = model.PkTienda,
                Ciudad = model.Ciudad,
                Estado = model.Estado
            };

            _context.Seccion.Add(seccion);
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

        // PUT: api/Secciones/Desactivar/1
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Desactivar([FromRoute] int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var seccion = await _context.Seccion.FirstOrDefaultAsync(s => s.IdSeccion == id);

            if (seccion == null)
            {
                return NotFound();
            }

            seccion.Estado = "Inactivo";

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

        // PUT: api/Secciones/Activar/1
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Activar([FromRoute] int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var seccion = await _context.Seccion.FirstOrDefaultAsync(s => s.IdSeccion == id);

            if (seccion == null)
            {
                return NotFound();
            }

            seccion.Estado = "Activo";

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
