using Sistema.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sistema.Infrastructure.Data.Configuration
{
    public class LicenciasConfiguration : IEntityTypeConfiguration<Licencias>
    {
        public void Configure(EntityTypeBuilder<Licencias> builder)
        {
            builder.ToTable("Licencias")
                .HasKey(l => l.IdLicencia); 
            builder.Property(l => l.Programa)
                .HasMaxLength(50);
            builder.Property(l => l.Licencia)
                .HasMaxLength(100);
            builder.Property(l => l.TipoLicencia)
                .HasMaxLength(15);
            builder.Property(l => l.Estado)
                .HasMaxLength(10);
            builder.HasOne(l => l.proveedor)
                .WithMany(p => p.licencias)
                .HasForeignKey(i => i.IdProveedor);
            builder.Property(l => l.Moneda)
                .HasMaxLength(12);
        }
    }
}
