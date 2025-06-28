using Sistema.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sistema.Infrastructure.Data.Configuration
{
    public class CategoriasConfiguration : IEntityTypeConfiguration<Categorias>
    {
        public void Configure(EntityTypeBuilder<Categorias> builder)
        {
            builder.ToTable("Categorias")
               .HasKey(c => c.IdCategoria);
            builder.Property(c => c.Categoria)
                .HasMaxLength(20);
            builder.Property(c => c.Tipo)
                .HasMaxLength(15);
            builder.Property(c => c.Estado)
                .HasMaxLength(10);
        }
    }
}
