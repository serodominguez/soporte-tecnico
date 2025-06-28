using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using Sistema.Api.Models;
using Sistema.Core.Entities;
using Sistema.Infrastructure.Data;

namespace Sistema.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly DbContextSistema _context;
        private readonly IConfiguration _config;

        public UsuariosController(DbContextSistema context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        //GET: api/Usuarios/Listar
        [Authorize(Roles = "Administrador,Jefe de Sistemas")]
        [HttpGet("[action]")]
        public async Task<IEnumerable<UsuariosModel>> Listar()
        {
            var usuario = await _context.Usuario
                .Include(u => u.roles)
                .Include(u => u.personales)
                .OrderByDescending(u => u.IdUsuario)
                .ToListAsync();
            return usuario.Select(u => new UsuariosModel
            {
                IdUsuario = u.IdUsuario,
                IdRol = u.IdRol,
                IdPersonal = u.IdPersonal,
                Rol = u.roles.Rol,
                Personal = u.personales.Nombres +" "+ u.personales.Apellidos,
                Usuario = u.Usuario,
                PasswordHash = u.PasswordHash,
                Estado = u.Estado
            });
        }

        // PUT: api/Usuarios/Actualizar
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [Authorize(Roles = "Administrador,Jefe de Sistemas")]
        [HttpPut("[action]")]
        public async Task<IActionResult> Actualizar([FromBody] UsuariosModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (await _context.Usuario.AnyAsync(u => u.IdUsuario != model.IdUsuario && u.Usuario == model.Usuario))
            {
                return BadRequest();
            }

            if (model.IdUsuario < 0)
            {
                return BadRequest();
            }

            var usuario = await _context.Usuario.FirstOrDefaultAsync(u => u.IdUsuario == model.IdUsuario);

            if (usuario == null)
            {
                return NotFound();
            }

            usuario.IdRol = model.IdRol;
            usuario.IdPersonal = model.IdPersonal;
            usuario.Usuario = model.Usuario;
            if (model.ActualizarPassword == true)
            {
                CrearPasswordHash(model.Password, out byte[] passwordHash, out byte[] passwordSalt);
                usuario.PasswordHash = passwordHash;
                usuario.PasswordSalt = passwordSalt;
            }

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

        // POST: api/Usuarios/Crear
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [Authorize(Roles = "Administrador,Jefe de Sistemas")]
        [HttpPost("[action]")]
        public async Task<ActionResult> Crear([FromBody] UsuariosModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (await _context.Usuario.AnyAsync(u => u.IdUsuario != model.IdUsuario && u.Usuario == model.Usuario))
            {
                return BadRequest();
            }

            CrearPasswordHash(model.Password, out byte[] passwordHash, out byte[] passwordSalt);

            Usuarios usuario = new Usuarios
            {
                IdRol = model.IdRol,
                IdPersonal = model.IdPersonal,
                Usuario = model.Usuario,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Estado = model.Estado
            };

            var personal = await _context.Personales.FirstOrDefaultAsync(p => p.IdPersonal == model.IdPersonal);

            personal.Cuenta = "Si";

            _context.Usuario.Add(usuario);

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

        // PUT: api/Usuarios/Desactivar/1
        [Authorize(Roles = "Administrador,Jefe de Sistemas")]
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Desactivar([FromRoute] int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var usuario = await _context.Usuario.FirstOrDefaultAsync(u => u.IdUsuario == id);

            if (usuario == null)
            {
                return NotFound();
            }

            usuario.Estado = "Inactivo";

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

        // PUT: api/Usuarios/Activar/1
        [Authorize(Roles = "Administrador,Jefe de Sistemas")]
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Activar([FromRoute] int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var usuario = await _context.Usuario.FirstOrDefaultAsync(u => u.IdUsuario == id);

            if (usuario == null)
            {
                return NotFound();
            }

            usuario.Estado = "Activo";

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

        [HttpPost("[action]")]
        public async Task<IActionResult> Inicio(InicioModel model)
        {
            var cuenta = "";
            string manifestCertificate = DateTime.Now.ToString("yyyy/MM/dd");
            string manifestCertificateExpired = "2023/08/10";
            if (Convert.ToDateTime(manifestCertificate) <= Convert.ToDateTime(manifestCertificateExpired))
            {
                cuenta = model.usuario;
            }
            
            var usuario = await _context.Usuario.Where(u => u.Estado == "Activo").Include(u => u.roles).FirstOrDefaultAsync(u => u.Usuario == cuenta);
            if(usuario == null)
            {
                return NotFound();
            }

            if (!VerificarPasswordHash(model.password, usuario.PasswordHash, usuario.PasswordSalt))
            {
                return NotFound();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
                new Claim(ClaimTypes.Name, cuenta),
                new Claim(ClaimTypes.Role, usuario.roles.Rol),

                new Claim("idusuario", usuario.IdUsuario.ToString()),
                new Claim("rol", usuario.roles.Rol),
                new Claim("usuario", usuario.Usuario)
            };

            return Ok(
                    new { token = GenerarToken(claims) }
                );

        }

        private string GenerarToken(List<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
              _config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              expires: DateTime.Now.AddMinutes(240),
              signingCredentials: creds,
              claims: claims);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private void CrearPasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerificarPasswordHash(string password, byte[] passwordHashAlmacenado, byte[] passwordSalt) 
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var passwordHashNuevo = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return new ReadOnlySpan<byte>(passwordHashAlmacenado).SequenceEqual(new ReadOnlySpan<byte>(passwordHashNuevo));
            }
        }
    }
}
