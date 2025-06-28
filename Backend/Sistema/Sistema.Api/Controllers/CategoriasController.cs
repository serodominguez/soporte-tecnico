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
    public class CategoriasController : ControllerBase
    {
        private readonly DbContextSistema _context;

        public CategoriasController(DbContextSistema context)
        {
            _context = context;
        }

        //GET: api/Categorias/Listar
        [HttpGet("[action]")]
        public async Task<IEnumerable<CategoriasModel>> Listar()
        {
            var categoria = await _context.Categoria.OrderByDescending(c => c.IdCategoria).ToListAsync();
            return categoria.Select(c => new CategoriasModel
            {
                IdCategoria = c.IdCategoria,
                Categoria = c.Categoria,
                Tipo = c.Tipo,
                Estado = c.Estado
            });
        }

        // GET: api/Categorias/SeleccionarEquipos
        [HttpGet("[action]")]
        public async Task<IEnumerable<CategoriasModel>> SeleccionarEquipos()
        {
            var categoria = await _context.Categoria.Where(c => c.Tipo == "EQUIPOS" && c.Estado == "Activo").OrderBy(c => c.Categoria).ToListAsync();
            return categoria.Select(c => new CategoriasModel
            {
                IdCategoria = c.IdCategoria,
                Categoria = c.Categoria
            });
        }

        // GET: api/Categorias/SeleccionarPerifericos
        [HttpGet("[action]")]
        public async Task<IEnumerable<CategoriasModel>> SeleccionarAccesorios()
        {
            var categoria = await _context.Categoria.Where(c => c.Tipo == "ACCESORIOS" && c.Estado == "Activo").OrderBy(c => c.Categoria).ToListAsync();
            return categoria.Select(c => new CategoriasModel
            {
                IdCategoria = c.IdCategoria,
                Categoria = c.Categoria
            });
        }


        // PUT: api/Categorias/Actualizar
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("[action]")]
        public async Task<IActionResult> Actualizar([FromBody] CategoriasModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (await _context.Categoria.AnyAsync(c => c.IdCategoria != model.IdCategoria && c.Categoria == model.Categoria))
            {
                return BadRequest();
            }

            if (model.IdCategoria < 0)
            {
                return BadRequest();
            }

            var categoria = await _context.Categoria.FirstOrDefaultAsync(c => c.IdCategoria == model.IdCategoria);

            if (categoria == null)
            {
                return NotFound();
            }

            categoria.Categoria = model.Categoria;
            categoria.Tipo = model.Tipo;

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

        // POST: api/Categorias/Crear
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("[action]")]
        public async Task<ActionResult> Crear([FromBody] CategoriasModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (await _context.Categoria.AnyAsync(c => c.IdCategoria != model.IdCategoria && c.Categoria == model.Categoria))
            {
                return BadRequest();
            }

            Categorias categoria = new Categorias
            {
                Categoria = model.Categoria,
                Tipo = model.Tipo,
                Estado = model.Estado
            };

            _context.Categoria.Add(categoria);
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

        // PUT: api/Categorias/Desactivar/1
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Desactivar([FromRoute] int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var categoria = await _context.Categoria.FirstOrDefaultAsync(c => c.IdCategoria == id);

            if (categoria == null)
            {
                return NotFound();
            }

            categoria.Estado = "Inactivo";

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

        // PUT: api/Categorias/Activar/1
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Activar([FromRoute] int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var categoria = await _context.Categoria.FirstOrDefaultAsync(c => c.IdCategoria == id);

            if (categoria == null)
            {
                return NotFound();
            }

            categoria.Estado = "Activo";

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
