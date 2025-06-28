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
    //[Authorize(Roles = "Administrador,Jefe de Sistemas,Soporte Fabrica,Soporte Tienda")]
    [Route("api/[controller]")]
    [ApiController]
    public class EquiposController : ControllerBase
    {
        private readonly DbContextSistema _context;

        public EquiposController(DbContextSistema context)
        {
            _context = context;
        }

        //GET: api/Equipos/ListarA
        [HttpGet("[action]")]
        public async Task<IEnumerable<EquiposModel>> ListarA()
        {
            var equipo = await _context.Equipo
                .Include(e => e.marcas)
                .Include(e => e.categorias)
                .OrderByDescending(e => e.IdEquipo)
                .Where(e => e.Tipo == 1)
                .ToListAsync();
            return equipo.Select(e => new EquiposModel
            {
                IdEquipo= e.IdEquipo,
                IdMarca = e.IdMarca,
                Marca = e.marcas.Marca,
                IdCategoria = e.IdCategoria,
                Categoria = e.categorias.Categoria,
                Modelo = e.Modelo,
                Serie = e.Serie,
                FechaCompra = e.FechaCompra.ToString("dd'/'MM'/'yyyy"),
                PrecioCompra = e.PrecioCompra,
                MesesGarantia = e.MesesGarantia,
                SistemaOperativo = e.SistemaOperativo,
                CodigoActivo = e.CodigoActivo,
                NombreEquipo = e.NombreEquipo,
                Procesador = e.Procesador,
                MemoriaRam = e.MemoriaRam,
                Almacenamiento = e.Almacenamiento,
                TarjetaVideo = e.TarjetaVideo,
                Condicion = e.Condicion,
                Asignado = e.Asignado,
                Estado = e.Estado,
                Moneda = e.Moneda
            });
        }

        //GET: api/Equipos/ListarB
        [HttpGet("[action]")]
        public async Task<IEnumerable<EquiposModel>> ListarB()
        {
            var equipo = await _context.Equipo
                .Include(e => e.marcas)
                .Include(e => e.categorias)
                .OrderByDescending(e => e.IdEquipo)
                .Where(e => e.Tipo == 2)
                .ToListAsync();
            return equipo.Select(e => new EquiposModel
            {
                IdEquipo = e.IdEquipo,
                IdMarca = e.IdMarca,
                Marca = e.marcas.Marca,
                IdCategoria = e.IdCategoria,
                Categoria = e.categorias.Categoria,
                Modelo = e.Modelo,
                Serie = e.Serie,
                FechaCompra = e.FechaCompra.ToString("dd'/'MM'/'yyyy"),
                PrecioCompra = e.PrecioCompra,
                MesesGarantia = e.MesesGarantia,
                CodigoActivo = e.CodigoActivo,
                Condicion = e.Condicion,
                Asignado = e.Asignado,
                Estado = e.Estado,
                Moneda = e.Moneda
            });
        }
        //GET: api/Equipos/EquipoIngreso
        [HttpGet("[action]/{texto}")]
        public async Task<IEnumerable<EquiposModel>> EquipoIngreso([FromRoute] string texto)
        {
            var equipo = await _context.Equipo
                .Include(e => e.marcas)
                .Include(e => e.categorias)
                .Where(e => e.Serie.Contains(texto) || e.CodigoActivo.Contains(texto))
                .Where(e => e.Estado == "Creado")
                .ToListAsync();
            return equipo.Select(e => new EquiposModel
            {
                IdEquipo = e.IdEquipo,
                IdCategoria = e.IdCategoria,
                Categoria = e.categorias.Categoria,
                IdMarca = e.IdMarca,
                Marca = e.marcas.Marca,
                Modelo = e.Modelo,
                Serie = e.Serie,
                CodigoActivo = e.CodigoActivo,
                Condicion = e.Condicion,
                Estado = e.Estado
            });
        }
        //GET: api/Equipos/EquipoEntrega
        [HttpGet("[action]/{texto}")]
        public async Task<IEnumerable<EquiposModel>> EquipoEntrega([FromRoute] string texto)
        {
            var equipo = await _context.Equipo
                .Include(e => e.marcas)
                .Include(e => e.categorias)
                .Where(e => e.Serie.Contains(texto) || e.CodigoActivo.Contains(texto))
                .Where(e => e.Asignado == "No" && e.Estado =="Activo")
                .ToListAsync();
            return equipo.Select(e => new EquiposModel
            {
                IdEquipo = e.IdEquipo,
                IdCategoria = e.IdCategoria,
                Categoria = e.categorias.Categoria,
                IdMarca = e.IdMarca,
                Marca = e.marcas.Marca,
                Modelo = e.Modelo,
                Serie = e.Serie,
                CodigoActivo = e.CodigoActivo,
                PrecioCompra = e.PrecioCompra,
                Condicion = e.Condicion,
                Estado = e.Estado
            });
        }

        //GET: api/Equipos/BuscarSerie
        [HttpGet("[action]/{serie}")]
        public async Task<IActionResult> BuscarSerie([FromRoute] string serie)
        {
            var equipo = await _context.Equipo
                .Include(p => p.marcas)
                .Include(e => e.categorias)
                .Where(e => e.Estado == "Creado")
                .SingleOrDefaultAsync(e => e.Serie == serie);

            if (equipo == null)
            { 
                return NotFound();
            }

            return Ok(new EquiposModel
            {
                IdEquipo = equipo.IdEquipo,
                IdCategoria = equipo.IdCategoria,
                Categoria = equipo.categorias.Categoria,
                IdMarca = equipo.IdMarca,
                Marca = equipo.marcas.Marca,
                Modelo = equipo.Modelo,
                Serie = equipo.Serie,
                CodigoActivo = equipo.CodigoActivo,
                Condicion = equipo.Condicion,
                Estado = equipo.Estado
            });
        }

        // PUT: api/Equipos/ActualizarA
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("[action]")]
        public async Task<IActionResult> ActualizarA([FromBody] EquiposModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model.IdEquipo < 0)
            {
                return BadRequest();
            }

            if (await _context.Equipo.AnyAsync(p => p.IdEquipo != model.IdEquipo && p.Serie == model.Serie))
            {
                return BadRequest();
            }

            var equipo = await _context.Equipo.FirstOrDefaultAsync(e => e.IdEquipo == model.IdEquipo);

            if (equipo == null)
            {
                return NotFound();
            }

            equipo.IdMarca = model.IdMarca;
            equipo.IdCategoria = model.IdCategoria;
            equipo.Modelo = model.Modelo;
            equipo.Serie = model.Serie;
            equipo.FechaCompra = Convert.ToDateTime(model.FechaCompra);
            equipo.PrecioCompra = model.PrecioCompra;
            equipo.MesesGarantia = model.MesesGarantia;
            equipo.SistemaOperativo = model.SistemaOperativo;
            equipo.CodigoActivo = model.CodigoActivo;
            equipo.NombreEquipo = model.NombreEquipo;
            equipo.Procesador = model.Procesador;
            equipo.MemoriaRam = model.MemoriaRam;
            equipo.Almacenamiento = model.Almacenamiento;
            equipo.TarjetaVideo = model.TarjetaVideo;
            equipo.Condicion = model.Condicion;
            equipo.Moneda = model.Moneda;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                //Console.WriteLine(ex.InnerException.Message);
                return BadRequest();
            }

            return Ok();
        }

        // PUT: api/Equipos/ActualizarB
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("[action]")]
        public async Task<IActionResult> ActualizarB([FromBody] EquiposModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model.IdEquipo < 0)
            {
                return BadRequest();
            }

            if (await _context.Equipo.AnyAsync(p => p.IdEquipo != model.IdEquipo && p.Serie == model.Serie))
            {
                return BadRequest();
            }

            var equipo = await _context.Equipo.FirstOrDefaultAsync(e => e.IdEquipo == model.IdEquipo);

            if (equipo == null)
            {
                return NotFound();
            }

            equipo.IdMarca = model.IdMarca;
            equipo.IdCategoria = model.IdCategoria;
            equipo.Modelo = model.Modelo;
            equipo.Serie = model.Serie;
            equipo.FechaCompra = Convert.ToDateTime(model.FechaCompra);
            equipo.PrecioCompra = model.PrecioCompra;
            equipo.MesesGarantia = model.MesesGarantia;
            equipo.CodigoActivo = model.CodigoActivo;
            equipo.Condicion = model.Condicion;
            equipo.Moneda = model.Moneda;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                //Console.WriteLine(ex.InnerException.Message);
                return BadRequest();
            }

            return Ok();
        }

        // POST: api/Equipos/Crear
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("[action]")]
        public async Task<ActionResult> CrearA([FromBody] EquiposModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (await _context.Equipo.AnyAsync(e => e.IdEquipo != model.IdEquipo && e.Serie == model.Serie))
            {
                return BadRequest();
            }

            Equipos equipo = new Equipos
            {
                IdCategoria = model.IdCategoria,
                IdMarca = model.IdMarca,
                IdSeccion = 96,
                Modelo = model.Modelo,
                Serie = model.Serie,
                FechaCompra = Convert.ToDateTime(model.FechaCompra),
                PrecioCompra = model.PrecioCompra,
                MesesGarantia = model.MesesGarantia,
                SistemaOperativo = model.SistemaOperativo,
                CodigoActivo = model.CodigoActivo,
                NombreEquipo = model.NombreEquipo,
                Procesador = model.Procesador,
                MemoriaRam = model.MemoriaRam,
                Almacenamiento = model.Almacenamiento,
                TarjetaVideo = model.TarjetaVideo,
                Condicion = model.Condicion,
                Asignado = model.Asignado,
                Estado = model.Estado,
                Moneda = model.Moneda,
                Tipo = model.Tipo
            };

            _context.Equipo.Add(equipo);
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

        // POST: api/Equipos/Crear
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("[action]")]
        public async Task<ActionResult> CrearB([FromBody] EquiposModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (await _context.Equipo.AnyAsync(e => e.IdEquipo != model.IdEquipo && e.Serie == model.Serie))
            {
                return BadRequest();
            }

            Equipos equipo = new Equipos
            {
                IdCategoria = model.IdCategoria,
                IdMarca = model.IdMarca,
                IdSeccion = 96,
                Modelo = model.Modelo,
                Serie = model.Serie,
                FechaCompra = Convert.ToDateTime(model.FechaCompra),
                PrecioCompra = model.PrecioCompra,
                MesesGarantia = model.MesesGarantia,
                CodigoActivo = model.CodigoActivo,
                Condicion = model.Condicion,
                Asignado = model.Asignado,
                Estado = model.Estado,
                Moneda = model.Moneda,
                Tipo = model.Tipo
            };

            _context.Equipo.Add(equipo);
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

        // PUT: api/Equipos/Desactivar/1
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Desactivar([FromRoute] int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var equipo = await _context.Equipo.FirstOrDefaultAsync(e => e.IdEquipo == id);

            if (equipo == null)
            {
                return NotFound();
            }

            equipo.Estado = "Inactivo";

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

        // PUT: api/Equipos/Activar/1
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Activar([FromRoute] int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var equipo = await _context.Equipo.FirstOrDefaultAsync(e => e.IdEquipo == id);

            if (equipo == null)
            {
                return NotFound();
            }

            equipo.Estado = "Activo";

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
