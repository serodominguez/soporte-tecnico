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
    public class PersonalesController : ControllerBase
    {
        private readonly DbContextSistema _context;

        public PersonalesController(DbContextSistema context)
        {
            _context = context;
        }

        //GET: api/Personales/ListarPersonal
        [HttpGet("[action]")]
        public async Task<IEnumerable<PersonalModel>> ListarPersonal()
        {
            var personal = await _context.Personales
                .Include(p => p.secciones)
                .Where(p => p.Tipo != "CONSIGNATARIO")
                .OrderByDescending(p => p.IdPersonal)
                .ToListAsync();
            return personal.Select(p => new PersonalModel
            {
                IdPersonal = p.IdPersonal,
                IdSeccion = p.IdSeccion,
                Seccion = p.secciones.Seccion,
                Nombres = p.Nombres,
                Apellidos = p.Apellidos,
                Cargo = p.Cargo,
                Tipo = p.Tipo,
                Celular = p.Celular,
                Carnet = p.Carnet.ToString(),
                Direccion = p.Direccion,
                PkEmpleado = p.PkEmpleado,
                Estado = p.Estado
            }); ;
        }

        //GET: api/Personales/ListarConsignatario
        [HttpGet("[action]")]
        public async Task<IEnumerable<PersonalModel>> ListarConsignatario()
        {
            var personal = await _context.Personales
                .Include(p => p.secciones)
                .Where(p => p.Tipo == "CONSIGNATARIO")
                .OrderByDescending(p => p.IdPersonal)
                .ToListAsync();
            return personal.Select(p => new PersonalModel
            {
                IdPersonal = p.IdPersonal,
                IdSeccion = p.IdSeccion,
                Seccion = p.secciones.Seccion,
                Nombres = p.Nombres,
                Apellidos = p.Apellidos,
                Cargo = p.Cargo,
                Tipo = p.Tipo,
                Telefono = p.Telefono,
                Celular = p.Celular,
                Carnet = p.Carnet.ToString(),
                Direccion = p.Direccion,
                PkEmpleado = p.PkEmpleado,
                Estado = p.Estado
            });
        }

        // GET: api/Personales/Seleccionar
        [HttpGet("[action]")]
        public async Task<IEnumerable<PersonalModel>> Seleccionar()
        {
            var personal = await _context.Personales
                .Where(p => p.Tipo != "CONSIGNATARIO" && p.Estado == "Activo" &&  p.Cuenta =="No")
                .OrderByDescending(p => p.IdPersonal)
                .ToListAsync();

            return personal.Select(p => new PersonalModel
            {
                IdPersonal = p.IdPersonal,
                Personal = p.Nombres +" "+ p.Apellidos
            });
        }

        // GET: api/Personales/SeleccionarPersonal
        [HttpGet("[action]")]
        public async Task<IEnumerable<PersonalModel>> SeleccionarPersonal()
        {
            var personal = await _context.Personales
                .Where(p => p.Estado == "Activo")
                .OrderByDescending(p => p.IdPersonal)
                .ToListAsync();

            return personal.Select(p => new PersonalModel
            {
                IdPersonal = p.IdPersonal,
                Personal = p.Nombres +" "+ p.Apellidos
            });
        }

        // PUT: api/Personales/Actualizar
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("[action]")]
        public async Task<IActionResult> Actualizar([FromBody] PersonalModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model.IdPersonal < 0)
            {
                return BadRequest();
            }

            if (model.PkEmpleado != null && model.PkEmpleado != "")
            {
                if (await _context.Personales.AnyAsync(p => p.IdPersonal != model.IdPersonal && p.PkEmpleado == model.PkEmpleado))
                {
                    return BadRequest();
                }
            }

            var personal = await _context.Personales.FirstOrDefaultAsync(p => p.IdPersonal == model.IdPersonal);

            if (personal == null)
            {
                return NotFound();
            }

            personal.IdSeccion = model.IdSeccion;
            personal.Nombres = model.Nombres;
            personal.Apellidos = model.Apellidos;
            personal.Cargo = model.Cargo;
            personal.Tipo = model.Tipo;
            personal.Telefono = model.Telefono;
            personal.Celular = model.Celular;
            personal.Carnet = Convert.ToInt32(model.Carnet);
            personal.Direccion = model.Direccion;
            personal.PkEmpleado = model.PkEmpleado;
            
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

        // POST: api/Personal/Crear
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("[action]")]
        public async Task<ActionResult> Crear([FromBody] PersonalModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model.PkEmpleado != null && model.PkEmpleado != "")
            {
                if (await _context.Personales.AnyAsync(p => p.IdPersonal != model.IdPersonal && p.Carnet == Convert.ToInt32(model.Carnet)))
                {
                    return BadRequest();
                }
            }

            Personal personal = new Personal
            {
                IdSeccion = model.IdSeccion,
                Nombres = model.Nombres,
                Apellidos = model.Apellidos,
                Cargo = model.Cargo,
                Tipo = model.Tipo,
                Telefono = model.Telefono,
                Celular = model.Celular,
                Carnet = Convert.ToInt32(model.Carnet),
                Direccion = model.Direccion,
                PkEmpleado = model.PkEmpleado,
                Cuenta = model.Cuenta,
                Estado = model.Estado
            };

            _context.Personales.Add(personal);
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

        // PUT: api/Personal/Desactivar/1
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Desactivar([FromRoute] int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var personal = await _context.Personales.FirstOrDefaultAsync(p => p.IdPersonal == id);

            if (personal == null)
            {
                return NotFound();
            }

            personal.Estado = "Inactivo";

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

        // PUT: api/Personal/Activar/1
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Activar([FromRoute] int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var personal = await _context.Personales.FirstOrDefaultAsync(p => p.IdPersonal == id);

            if (personal == null)
            {
                return NotFound();
            }

            personal.Estado = "Activo";

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
