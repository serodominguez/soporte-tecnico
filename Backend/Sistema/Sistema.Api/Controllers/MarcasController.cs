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
    public class MarcasController : ControllerBase
    {
        private readonly DbContextSistema _context;

        public MarcasController(DbContextSistema context)
        {
            _context = context;
        }

        //GET: api/Marcas/Listar
        [HttpGet("[action]")]
        public async Task<IEnumerable<MarcasModel>> Listar()
        {
            var marca = await _context.Marca.OrderByDescending(m => m.IdMarca).ToListAsync();
            return marca.Select(m => new MarcasModel
            {
                IdMarca = m.IdMarca,
                Marca = m.Marca,
                Tipo = m.Tipo,
                Estado = m.Estado
            });
        }

        // GET: api/Marcas/Seleccionar
        [HttpGet("[action]")]
        public async Task<IEnumerable<MarcasModel>> Seleccionar()
        {
            var marca = await _context.Marca.Where(m => m.Estado == "Activo").OrderBy(m => m.Marca).ToListAsync();
            return marca.Select(m => new MarcasModel
            {
                IdMarca = m.IdMarca,
                Marca = m.Marca
            });
        }

        // PUT: api/Marcas/Actualizar
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("[action]")]
        public async Task<IActionResult> Actualizar([FromBody] MarcasModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (await _context.Marca.AnyAsync(m => m.IdMarca != model.IdMarca && m.Marca == model.Marca))
            {
                return BadRequest();
            }

            if (model.IdMarca < 0)
            {
                return BadRequest();
            }

            var marca = await _context.Marca.FirstOrDefaultAsync(m => m.IdMarca == model.IdMarca);

            if (marca == null)
            {
                return NotFound();
            }

            marca.Marca = model.Marca;
            marca.Tipo = model.Tipo;

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

        // POST: api/Marca/Crear
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("[action]")]
        public async Task<ActionResult> Crear([FromBody] MarcasModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (await _context.Marca.AnyAsync(m => m.IdMarca != model.IdMarca && m.Marca == model.Marca))
            {
                return BadRequest();
            }

            Marcas marca = new Marcas
            {
                Marca = model.Marca,
                Tipo = model.Tipo,
                Estado = model.Estado
            };

            _context.Marca.Add(marca);
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

        // PUT: api/Marcas/Desactivar/1
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Desactivar([FromRoute] int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var marca = await _context.Marca.FirstOrDefaultAsync(m => m.IdMarca == id);

            if (marca == null)
            {
                return NotFound();
            }

            marca.Estado = "Inactivo";

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

        // PUT: api/Marcas/Activar/1
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Activar([FromRoute] int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var marca = await _context.Marca.FirstOrDefaultAsync(m => m.IdMarca == id);

            if (marca == null)
            {
                return NotFound();
            }

            marca.Estado = "Activo";

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
