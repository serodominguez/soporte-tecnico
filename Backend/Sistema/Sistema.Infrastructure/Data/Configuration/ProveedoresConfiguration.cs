using Sistema.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sistema.Infrastructure.Data.Configuration
{
    public class ProveedoresConfiguration : IEntityTypeConfiguration<Proveedores>
    {
        public void Configure(EntityTypeBuilder<Proveedores> builder)
        {
            builder.ToTable("Proveedores")
                .HasKey(p => p.IdProveedor);
            builder.Property(p => p.RazonSocial)
                .HasMaxLength(80);
            builder.Property(p => p.Contacto)
                .HasMaxLength(60);
            builder.Property(p => p.Celular)
                .HasMaxLength(20);
            builder.Property(p => p.Telefono)
                .HasMaxLength(20);
            builder.Property(p => p.Correo)
                .HasMaxLength(25);
            builder.Property(p => p.Estado)
                .HasMaxLength(10);
        }
    }
}
