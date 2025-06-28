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
    public class LicenciasController : ControllerBase
    {
        private readonly DbContextSistema _context;

        public LicenciasController(DbContextSistema context)
        {
            _context = context;
        }
        //GET: api/Licencias/Listar
        [HttpGet("[action]")]
        public async Task<IEnumerable<LicenciasModel>> Listar()
        {
            var licencia = await _context.Licencia
                .Include(l => l.proveedor)
                .OrderByDescending(l => l.IdLicencia)
                .ToListAsync();
            return licencia.Select(l => new LicenciasModel
            {
                IdLicencia = l.IdLicencia,
                Programa = l.Programa,
                Licencia = l.Licencia,
                TipoLicencia = l.TipoLicencia,
                CantidadEquipos = l.CantidadEquipos,
                PrecioCompra = l.PrecioCompra.ToString(),
                FechaCompra = l.FechaCompra.HasValue ? l.FechaCompra.Value.ToString("dd'/'MM'/'yyyy") : "",
                FechaCaducidad = l.FechaCaducidad.HasValue ? l.FechaCaducidad.Value.ToString("dd'/'MM'/'yyyy") : "",
                FechaActivacion = l.FechaActivacion.HasValue ? l.FechaActivacion.Value.ToString("dd'/'MM'/'yyyy") : "",
                Estado = l.Estado,
                IdProveedor = l.IdProveedor,
                Proveedor = l.proveedor.RazonSocial,
                Moneda = l.Moneda,
                Comentarios = l.Comentarios
            });
        }

        // PUT: api/Licencias/Actualizar
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("[action]")]
        public async Task<IActionResult> Actualizar([FromBody] LicenciasModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model.IdLicencia < 0)
            {
                return BadRequest();
            }

            var licencia = await _context.Licencia.FirstOrDefaultAsync(l => l.IdLicencia == model.IdLicencia);

            if (licencia == null)
            {
                return NotFound();
            }

            licencia.Programa = model.Programa;
            licencia.Licencia = model.Licencia;
            licencia.TipoLicencia = model.TipoLicencia;
            licencia.CantidadEquipos = model.CantidadEquipos;
            licencia.PrecioCompra = Convert.ToInt32(model.PrecioCompra);
            licencia.FechaCompra = Convert.ToDateTime(model.FechaCompra);
            licencia.FechaCaducidad = Convert.ToDateTime(model.FechaCaducidad);
            licencia.FechaActivacion = Convert.ToDateTime(model.FechaActivacion);
            licencia.IdProveedor = model.IdProveedor;
            licencia.Moneda = model.Moneda;
            licencia.Comentarios = model.Comentarios;

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

        // POST: api/Licencias/Crear
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("[action]")]
        public async Task<ActionResult> Crear([FromBody] LicenciasModel model)
        {
            int? precio = null;
            DateTime? fechaCo = null;
            DateTime? fechaCa = null;
            DateTime? fechaAc = null;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model.PrecioCompra != "" && model.PrecioCompra != null)
            {
                precio = Convert.ToInt32(model.PrecioCompra);
            }

            if (model.FechaCompra != "" && model.FechaCompra != null)
            {
                fechaCo = Convert.ToDateTime(model.FechaCompra);
            }

            if (model.FechaCaducidad != "" && model.FechaCaducidad != null)
            {
                fechaCa = Convert.ToDateTime(model.FechaCaducidad);
            }

            if (model.FechaActivacion != "" && model.FechaActivacion != null)
            {
                fechaAc = Convert.ToDateTime(model.FechaActivacion);
            }

            Licencias licencia = new Licencias
            {
                Programa = model.Programa,
                Licencia = model.Licencia,
                TipoLicencia = model.TipoLicencia,
                CantidadEquipos = model.CantidadEquipos,
                PrecioCompra = precio,
                FechaCompra = fechaCo,
                FechaCaducidad = fechaCa,
                FechaActivacion = fechaAc,
                Estado = model.Estado,
                IdProveedor = model.IdProveedor,
                Moneda = model.Moneda,
                Comentarios = model.Comentarios
            };

            _context.Licencia.Add(licencia);
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

        // PUT: api/Licencias/Desactivar/1
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Desactivar([FromRoute] int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var licencia = await _context.Licencia.FirstOrDefaultAsync(l => l.IdLicencia == id);

            if (licencia == null)
            {
                return NotFound();
            }

            licencia.Estado = "Inactivo";

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

        // PUT: api/Licencias/Activar/1
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Activar([FromRoute] int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var licencia = await _context.Licencia.FirstOrDefaultAsync(l => l.IdLicencia == id);

            if (licencia == null)
            {
                return NotFound();
            }

            licencia.Estado = "Activo";

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
