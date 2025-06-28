using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sistema.Api.Models;
using Sistema.Api.Util;
using Sistema.Core.Entities;
using Sistema.Infrastructure.Data;

namespace Sistema.Api.Controllers
{
    [Authorize(Roles = "Administrador,Jefe de Sistemas,Soporte Fabrica,Soporte Tienda")]
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentosController : ControllerBase
    {
        private readonly DbContextSistema _context;
        public DocumentosController(DbContextSistema context)
        {
            _context = context;
        }
        
        //GET: api/Documentos/ListarEntrega
        [HttpGet("[action]")]
        public async Task<IEnumerable<DocumentosModel>> ListarEntrega()
        {
            var documento = await _context.Documento
                .Include(d => d.usuario)
                .Include(d => d.personal)
                .Include(d => d.seccion)
                .Where(d => d.Estado == "Entregado")
                .OrderByDescending(d => d.IdDocumento)
                .ToListAsync();

            return documento.Select(d => new DocumentosModel
            {
                IdDocumento = d.IdDocumento,
                IdPersonal = d.IdPersonal,
                IdSeccion = d.IdSeccion,
                Personal = d.personal.Nombres + " " + d.personal.Apellidos,
                PkEmpleado = d.personal.PkEmpleado,
                Cargo = d.personal.Cargo,
                Celular = d.personal.Celular,
                Carnet = d.personal.Carnet,
                Direccion = d.personal.Direccion,
                Usuario = d.usuario.Usuario,
                Seccion = d.seccion.Seccion,
                FechaEntrega = d.FechaEntrega.ToString("dd'/'MM'/'yyyy"),
                NumeroEntrega = d.NumeroEntrega,
                Total = d.Total,
                Estado = d.Estado,
            });
        }

        //GET: api/Documentos/ListarDevoluciones
        [HttpGet("[action]")]
        public async Task<IEnumerable<DocumentosModel>> ListarDevoluciones()
        {
            var documento = await _context.Documento
                .Include(d => d.usuario)
                .Include(d => d.personal)
                .Include(d => d.seccion)
                .Where(d => d.Estado == "Entregado" || d.Estado == "Devuelto")
                .OrderByDescending(d => d.IdDocumento)
                .ToListAsync();

            return documento.Select(d => new DocumentosModel
            {
                IdDocumento = d.IdDocumento,
                IdPersonal = d.IdPersonal,
                IdSeccion = d.IdSeccion,
                Personal = d.personal.Nombres + " " + d.personal.Apellidos,
                PkEmpleado = d.personal.PkEmpleado,
                Cargo = d.personal.Cargo,
                Celular = d.personal.Celular,
                Usuario = d.usuario.Usuario,
                Seccion = d.seccion.Seccion,
                FechaEntrega = d.FechaEntrega.ToString("dd'/'MM'/'yyyy"),
                FechaDevolucion = d.FechaDevolucion.HasValue ? d.FechaDevolucion.Value.ToString("dd'/'MM'/'yyyy") : "",
                NumeroEntrega = d.NumeroEntrega,
                NumeroDevolucion = d.NumeroDevolucion,
                Observaciones = d.Observaciones,
                Total = d.Total,
                Estado = d.Estado,
            });
        }

        //GET: api/Documentos/ListarDetalles
        [HttpGet("[action]/{idingreso}")]
        public async Task<IEnumerable<DetalleDocumentosModel>> ListarDetalles([FromRoute] int idingreso)
        {
            var detalle = await _context.DetalleDocumento
                .Include(e => e.equipo)
                .Where(d => d.IdDocumento == idingreso)
                .ToListAsync();

            return detalle.Select(d => new DetalleDocumentosModel
            {
                IdEquipo = d.IdEquipo,
                Marca = d.Marca,
                Categoria = d.Categoria,
                Modelo = d.equipo.Modelo,
                Serie = d.equipo.Serie,
                CodigoActivo = d.CodigoActivo,
                PrecioCompra = d.PrecioCompra
            });
        }

        // POST: api/Documentos/Crear
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("[action]")]
        public async Task<ActionResult> Crear([FromBody] DocumentosModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var numero = await _context.Documento
                .CountAsync() + 1;

            var fecha = DateTime.Now;

            Documentos documento = new Documentos
            {
                IdPersonal = model.IdPersonal,
                IdUsuario = Convert.ToInt32(model.IdUsuario),
                IdSeccion = model.IdSeccion,
                Total = model.Total,
                FechaEntrega = fecha,
                NumeroEntrega = numero,
                Estado = "Entregado"
            };

            try
            {
                _context.Documento.Add(documento);
                await _context.SaveChangesAsync();

                var id = documento.IdDocumento;
                foreach (var d in model.detalles)
                {
                    DetalleDocumentos detalle = new DetalleDocumentos
                    {
                        IdDocumento = id,
                        IdEquipo = d.IdEquipo,
                        Marca = d.Marca,
                        Categoria = d.Categoria,
                        Modelo = d.Modelo,
                        Serie = d.Serie,
                        CodigoActivo = d.CodigoActivo,
                        PrecioCompra = d.PrecioCompra
                    };

                    var equipo = await _context.Equipo.FirstOrDefaultAsync(e => e.IdEquipo == d.IdEquipo);
                    equipo.IdSeccion = model.IdSeccion;
                    equipo.Asignado = "Si";

                    _context.DetalleDocumento.Add(detalle);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

            return Ok();
        }

        // PUT: api/Documentos/Actualizar
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("[action]")]
        public async Task<IActionResult> Actualizar([FromBody] DocumentosModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model.IdDocumento < 0)
            {
                return BadRequest();
            }
            var numero = await _context.Documento
                .Where(d => d.Estado == "Devuelto")
                .CountAsync() + 1;

            var documento = await _context.Documento.FirstOrDefaultAsync(d => d.IdDocumento == model.IdDocumento);

            if (documento == null)
            {
                return NotFound();
            }

            var fecha = DateTime.Now;
            documento.FechaDevolucion = fecha;
            documento.NumeroDevolucion = numero;
            documento.Observaciones = model.Observaciones;
            documento.Estado = "Devuelto";

            try
            {
                foreach (var d in model.detalles)
                {
                    var equipo = await _context.Equipo.FirstOrDefaultAsync(e => e.IdEquipo == d.IdEquipo);
                    equipo.IdSeccion = 96;
                    equipo.Asignado = "No";
                }

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }

            return Ok();
        }

        // PUT: api/Documentos/Desactivar/1
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Anular([FromRoute] int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var documento = await _context.Documento.FirstOrDefaultAsync(i => i.IdDocumento == id);

            if (documento == null)
            {
                return NotFound();
            }

            documento.Estado = "Anulado";

            try
            {
                await _context.SaveChangesAsync();
                var detalle = await _context.DetalleDocumento
                    .Include(e => e.equipo)
                    .Where(d => d.IdDocumento == id)
                    .ToListAsync();

                foreach (var det in detalle)
                {
                    var equipo = await _context.Equipo.FirstOrDefaultAsync(e => e.IdEquipo == det.equipo.IdEquipo);
                    equipo.IdSeccion = 96;
                    equipo.Asignado = "No";
                    await _context.SaveChangesAsync();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }

            return Ok();
        }

        // POST: api/Documentos/Generar
        [HttpPost("[action]")]
        public ActionResult Generar([FromBody] DocumentosModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }


            GenerarPDF generarPDF = new GenerarPDF();
            string newDocumentFileName = generarPDF.GenerateInvestorDocument(model);

            byte[] DocumentoPDF = System.IO.File.ReadAllBytes(generarPDF.GenerateInvestorDocument(model));

            return new FileContentResult(DocumentoPDF, "application/pdf");

        }
    }
}
