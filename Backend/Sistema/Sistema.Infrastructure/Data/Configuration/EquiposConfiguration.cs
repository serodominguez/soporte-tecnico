using Sistema.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sistema.Infrastructure.Data.Configuration
{
    public class EquiposConfiguration : IEntityTypeConfiguration<Equipos>
    {
        public void Configure(EntityTypeBuilder<Equipos> builder)
        {
            builder.ToTable("Equipos")
                .HasKey(e => e.IdEquipo);
            builder.HasOne(m => m.marcas)
                .WithMany(e => e.equipos)
                .HasForeignKey(e => e.IdMarca);
            builder.HasOne(c => c.categorias)
                .WithMany(e => e.equipos)
                .HasForeignKey(e => e.IdCategoria);
            builder.HasOne(s => s.secciones)
                 .WithMany(e => e.equipos)
                 .HasForeignKey(e => e.IdSeccion);

        }
    }
}
