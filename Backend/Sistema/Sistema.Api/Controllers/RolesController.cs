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
    [Authorize(Roles = "Administrador,Jefe de Sistemas")]
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly DbContextSistema _context;

        public RolesController(DbContextSistema context)
        {
            _context = context;
        }

        //GET: api/Roles/Listar
        [HttpGet("[action]")]
        public async Task<IEnumerable<RolesModel>> Listar()
        {
            var rol = await _context.Rol.OrderBy(r => r.Rol).ToListAsync();
            return rol.Select(r => new RolesModel
            {
                IdRol = r.IdRol,
                Rol = r.Rol,
                Estado = r.Estado
            });
        }

        // GET: api/Roles/Seleccionar
        [HttpGet("[action]")]
        public async Task<IEnumerable<RolesModel>> Seleccionar()
        {
            var rol = await _context.Rol.Where(r => r.Estado == "Activo").ToListAsync();

            return rol.Select(r => new RolesModel
            {
                IdRol = r.IdRol,
                Rol = r.Rol
            });
        }

    }
}
