using Sistema.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sistema.Infrastructure.Data.Configuration
{
    public class MarcasConfiguration : IEntityTypeConfiguration<Marcas>
    {
        public void Configure(EntityTypeBuilder<Marcas> builder)
        {
            builder.ToTable("Marcas")
                .HasKey(m => m.IdMarca);
            builder.Property(m => m.Marca)
                .HasMaxLength(15);
            builder.Property(m => m.Tipo)
                .HasMaxLength(15);
            builder.Property(m => m.Estado)
                .HasMaxLength(10);
        }
    }
}
