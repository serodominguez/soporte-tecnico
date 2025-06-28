using Sistema.Infrastructure.Data.Configuration;
using Sistema.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sistema.Infrastructure.Data
{
    public class DbContextSistema : DbContext
    {
        public DbSet<Areas> Area { get; set; }
        public DbSet<Categorias> Categoria { get; set; }
        public DbSet<DetalleIngresos> DetalleIngreso { get; set; }
        public DbSet<DetalleDocumentos> DetalleDocumento { get; set; }
        public DbSet<Documentos> Documento { get; set; }
        public DbSet<Equipos> Equipo { get; set; }
        public DbSet<Ingresos> Ingreso { get; set; }
        public DbSet<Marcas> Marca { get; set; }
        public DbSet<Licencias> Licencia { get; set; }
        public DbSet<Personal> Personales { get; set; }
        public DbSet<Proveedores> Proveedor { get; set; }
        public DbSet<Roles> Rol { get; set; }
        public DbSet<Secciones> Seccion { get; set; }
        public DbSet<Usuarios> Usuario { get; set; }

        public DbContextSistema(DbContextOptions<DbContextSistema> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new AreasConfiguration());
            modelBuilder.ApplyConfiguration(new CategoriasConfiguration());
            modelBuilder.ApplyConfiguration(new DetalleIngresosConfiguration());
            modelBuilder.ApplyConfiguration(new DetalleDocumentosConfiguration());
            modelBuilder.ApplyConfiguration(new DocumentosConfiguration());
            modelBuilder.ApplyConfiguration(new EquiposConfiguration());
            modelBuilder.ApplyConfiguration(new IngresosConfiguration());
            modelBuilder.ApplyConfiguration(new LicenciasConfiguration());
            modelBuilder.ApplyConfiguration(new MarcasConfiguration());
            modelBuilder.ApplyConfiguration(new PersonalConfiguration());
            modelBuilder.ApplyConfiguration(new ProveedoresConfiguration());
            modelBuilder.ApplyConfiguration(new RolesConfiguration());
            modelBuilder.ApplyConfiguration(new SeccionesConfiguration());
            modelBuilder.ApplyConfiguration(new UsuariosConfiguration());
        }
    }
}
