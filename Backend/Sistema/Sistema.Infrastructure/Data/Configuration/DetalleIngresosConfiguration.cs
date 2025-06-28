using Sistema.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sistema.Infrastructure.Data.Configuration
{
    public class DetalleIngresosConfiguration : IEntityTypeConfiguration<DetalleIngresos>
    {
        public void Configure(EntityTypeBuilder<DetalleIngresos> builder)
        {
            builder.ToTable("DetalleIngresos")
                .HasKey(d => d.IdDetalleIngresos);
            builder.HasOne(d => d.ingreso)
                .WithMany(i => i.detallesIngreso)
                .HasForeignKey(d => d.IdIngreso);
            builder.HasOne(d => d.equipo)
                .WithMany(e => e.detallesIngreso)
                .HasForeignKey(d => d.IdEquipo);
        }
    }
}
