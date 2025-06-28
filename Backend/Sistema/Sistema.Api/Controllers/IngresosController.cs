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
    public class IngresosController : ControllerBase
    {
        private readonly DbContextSistema _context;

        public IngresosController(DbContextSistema context)
        {
            _context = context;
        }

        //GET: api/Ingresos/Listar
        [HttpGet("[action]")]
        public async Task<IEnumerable<IngresosModel>> Listar()
        {
            var ingreso = await _context.Ingreso
                .Include(i => i.usuario)
                .Include(i => i.proveedor)
                .OrderByDescending( i => i.IdIngreso)
                .ToListAsync();

            return ingreso.Select(i => new IngresosModel
            {
                IdIngreso = i.IdIngreso,
                IdProveedor = i.IdProveedor,
                Proveedor = i.proveedor.RazonSocial,
                Usuario = i.usuario.Usuario,
                FechaIngreso = i.FechaIngreso.ToString("dd'/'MM'/'yyyy"),
                NumeroIngreso = i.NumeroIngreso,
                TipoComprobante = i.TipoComprobante,
                NumeroComprobante = i.NumeroComprobante.ToString(),
                NumeroOrden = i.NumeroOrden,
                Autorizado = i.Autorizado,
                Observaciones = i.Observaciones,
                Estado = i.Estado
            });
        }

        //GET: api/Ingresos/ListarDetalles
        [HttpGet("[action]/{idingreso}")]
        public async Task<IEnumerable<DetalleIngresosModel>> ListarDetalles([FromRoute] int idingreso)
        {
            var detalle = await _context.DetalleIngreso
                .Include(e => e.equipo)
                .Where(d => d.IdIngreso == idingreso)
                .ToListAsync();

            return detalle.Select(d => new DetalleIngresosModel
            {
                IdEquipo = d.IdEquipo,
                Marca = d.Marca,
                Categoria = d.Categoria,
                Modelo = d.equipo.Modelo,
                Serie = d.equipo.Serie,
                CodigoActivo = d.equipo.CodigoActivo
            });
        }

        // POST: api/Ingresos/Crear
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("[action]")]
        public async Task<ActionResult> Crear([FromBody] IngresosModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var numero = await _context.Ingreso.CountAsync() + 1;

            var fecha = DateTime.Now;

            Ingresos ingreso = new Ingresos { 
                IdProveedor = model.IdProveedor,
                IdUsuario = Convert.ToInt32(model.IdUsuario),
                FechaIngreso = fecha,
                NumeroIngreso = numero,
                TipoComprobante = model.TipoComprobante,
                NumeroComprobante = Convert.ToInt32(model.NumeroComprobante),
                NumeroOrden = model.NumeroOrden,
                Autorizado = model.Autorizado,
                Observaciones = model.Observaciones,
                Estado = "Ingresado"
            };

            try
            {
                _context.Ingreso.Add(ingreso);
                await _context.SaveChangesAsync();

                var id = ingreso.IdIngreso;
                foreach (var d in model.detalles) 
                {
                    DetalleIngresos detalle = new DetalleIngresos
                    {
                        IdIngreso = id,
                        IdEquipo = d.IdEquipo,
                        Marca = d.Marca,
                        Categoria = d.Categoria,
                        Modelo = d.Modelo,
                        Serie = d.Serie,
                        CodigoActivo = d.CodigoActivo
                    };

                    var equipo = await _context.Equipo.FirstOrDefaultAsync(e => e.IdEquipo == d.IdEquipo);
                    equipo.Estado = "Activo";

                    _context.DetalleIngreso.Add(detalle);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

            return Ok();
        }

        // PUT: api/Ingresos/Desactivar/1
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Anular([FromRoute] int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var ingreso = await _context.Ingreso.FirstOrDefaultAsync(i => i.IdIngreso == id);

            if (ingreso == null)
            {
                return NotFound();
            }

            ingreso.Estado = "Anulado";

            try
            {
                await _context.SaveChangesAsync();
                var detalle = await _context.DetalleIngreso
                    .Include(e => e.equipo)
                    .Where(d => d.IdIngreso == id)
                    .ToListAsync();

                foreach (var det in detalle)
                {
                    var equipo = await _context.Equipo.FirstOrDefaultAsync(e => e.IdEquipo == det.equipo.IdEquipo);
                    equipo.Estado = "Creado";
                    await _context.SaveChangesAsync();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
               return BadRequest();
            }

            return Ok();
        }
    }
}
