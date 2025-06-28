using Sistema.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sistema.Infrastructure.Data.Configuration
{
    public class RolesConfiguration : IEntityTypeConfiguration<Roles>
    {
        public void Configure(EntityTypeBuilder<Roles> builder)
        {
            builder.ToTable("Roles")
                 .HasKey(r => r.IdRol);
            builder.Property(r => r.Rol)
                .HasMaxLength(20);
            builder.Property(r => r.Estado)
                .HasMaxLength(10);
        }
    }
}
