using Sistema.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sistema.Infrastructure.Data.Configuration
{
    public class AreasConfiguration : IEntityTypeConfiguration<Areas>
    {
        public void Configure(EntityTypeBuilder<Areas> builder)
        {
            builder.ToTable("Areas")
                .HasKey(a => a.IdArea);
            builder.Property(a => a.Area)
                .HasMaxLength(15);
            builder.Property(a => a.Tipo)
                .HasMaxLength(10);
            builder.Property(a => a.Estado)
                .HasMaxLength(10);
        }
    }
}
