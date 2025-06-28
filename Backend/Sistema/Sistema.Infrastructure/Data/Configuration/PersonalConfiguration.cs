using Sistema.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sistema.Infrastructure.Data.Configuration
{
    public class PersonalConfiguration : IEntityTypeConfiguration<Personal>
    {
        public void Configure(EntityTypeBuilder<Personal> builder)
        {
            builder.ToTable("Personal")
                .HasKey(p => p.IdPersonal);
            builder.HasOne(p => p.secciones)
                .WithMany(s => s.personales)
                .HasForeignKey(p => p.IdSeccion);
            builder.Property(p => p.Nombres)
                .HasMaxLength(30);
            builder.Property(p => p.Apellidos)
                .HasMaxLength(30);
            builder.Property(p => p.Cargo)
                .HasMaxLength(50);
            builder.Property(p => p.Tipo)
                .HasMaxLength(20);
            builder.Property(p => p.Telefono)
                .HasMaxLength(10);
            builder.Property(p => p.Celular)
                .HasMaxLength(10);
            builder.Property(p => p.PkEmpleado)
                .HasMaxLength(15);
            builder.Property(p => p.Estado)
                .HasMaxLength(10);
        }
    }
}
