using Sistema.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sistema.Infrastructure.Data.Configuration
{
    public class IngresosConfiguration : IEntityTypeConfiguration<Ingresos>
    {
        public void Configure(EntityTypeBuilder<Ingresos> builder)
        {
            builder.ToTable("Ingresos")
                 .HasKey(i => i.IdIngreso);
            builder.HasOne(i => i.usuario)
                .WithMany(u => u.ingresos)
                .HasForeignKey(i => i.IdUsuario);
            builder.HasOne(i => i.proveedor)
                .WithMany(p => p.ingresos)
                .HasForeignKey(i => i.IdProveedor);
        }
    }
}
