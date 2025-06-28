using Sistema.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sistema.Infrastructure.Data.Configuration
{
    public class SeccionesConfiguration : IEntityTypeConfiguration<Secciones>
    {
        public void Configure(EntityTypeBuilder<Secciones> builder)
        {
            builder.ToTable("Secciones")
                 .HasKey(s => s.IdSeccion);
            builder.HasOne(s => s.areas)
                .WithMany(a => a.secciones)
                .HasForeignKey(s => s.IdArea);
            builder.Property(s => s.Seccion)
                .HasMaxLength(30);
            builder.Property(s => s.Direccion)
                .HasMaxLength(30);
            builder.Property(s => s.Ciudad)
                .HasMaxLength(15);
            builder.Property(s => s.Estado)
                .HasMaxLength(10);
        }
    }
}
