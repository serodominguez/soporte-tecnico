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
    public class ProveedoresController : ControllerBase
    {
        private readonly DbContextSistema _context;

        public ProveedoresController(DbContextSistema context)
        {
            _context = context;
        }

        //GET: api/Proveedores/Listar
        [HttpGet("[action]")]
        public async Task<IEnumerable<ProveedoresModel>> Listar()
        {
            var proveedor = await _context.Proveedor.OrderByDescending(p => p.IdProveedor).ToListAsync();
            return proveedor.Select(p => new ProveedoresModel
            {
                IdProveedor = p.IdProveedor,
                RazonSocial = p.RazonSocial,
                Contacto = p.Contacto,
                Celular = p.Celular,
                Telefono = p.Telefono,
                Correo = p.Correo,
                Pais = p.Pais,
                Estado = p.Estado
            });
        }

        // GET: api/Proveedores/Seleccionar
        [HttpGet("[action]")]
        public async Task<IEnumerable<ProveedoresModel>> Seleccionar()
        {
            var proveedor = await _context.Proveedor.Where(p => p.Estado == "Activo").OrderBy(p => p.RazonSocial).ToListAsync();
            return proveedor.Select(p => new ProveedoresModel
            {
                IdProveedor = p.IdProveedor,
                RazonSocial = p.RazonSocial
            });
        }

        // PUT: api/Proveedores/Actualizar
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("[action]")]
        public async Task<IActionResult> Actualizar([FromBody] ProveedoresModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (await _context.Proveedor.AnyAsync(p => p.IdProveedor != model.IdProveedor && p.RazonSocial == model.RazonSocial))
            {
                return BadRequest();
            }

            if (model.IdProveedor < 0)
            {
                return BadRequest();
            }

            var proveedor = await _context.Proveedor.FirstOrDefaultAsync(p => p.IdProveedor == model.IdProveedor);

            if (proveedor == null)
            {
                return NotFound();
            }

            proveedor.RazonSocial = model.RazonSocial;
            proveedor.Contacto = model.Contacto;
            proveedor.Celular = model.Celular;
            proveedor.Telefono = model.Telefono;
            proveedor.Correo = model.Correo;
            proveedor.Pais = model.Pais;

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

        // POST: api/Proveedores/Crear
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("[action]")]
        public async Task<ActionResult> Crear([FromBody] ProveedoresModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (await _context.Proveedor.AnyAsync(p => p.IdProveedor != model.IdProveedor && p.RazonSocial == model.RazonSocial))
            {
                return BadRequest();
            }

            Proveedores proveedor = new Proveedores
            {
                RazonSocial = model.RazonSocial,
                Contacto = model.Contacto,
                Celular = model.Celular,
                Telefono = model.Telefono,
                Correo = model.Correo,
                Pais = model.Pais,
                Estado = model.Estado
            };

            _context.Proveedor.Add(proveedor);
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

        // PUT: api/Proveedores/Desactivar/1
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Desactivar([FromRoute] int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var proveedor = await _context.Proveedor.FirstOrDefaultAsync(p => p.IdProveedor == id);

            if (proveedor == null)
            {
                return NotFound();
            }

            proveedor.Estado = "Inactivo";

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

            var proveedor = await _context.Proveedor.FirstOrDefaultAsync(p => p.IdProveedor == id);

            if (proveedor == null)
            {
                return NotFound();
            }

            proveedor.Estado = "Activo";

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
