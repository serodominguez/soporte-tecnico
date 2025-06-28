using Sistema.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sistema.Infrastructure.Data.Configuration
{
    public class UsuariosConfiguration : IEntityTypeConfiguration<Usuarios>
    {
        public void Configure(EntityTypeBuilder<Usuarios> builder)
        {
            builder.ToTable("Usuarios")
                .HasKey(u => u.IdUsuario);
            builder.HasOne(r => r.roles)
                .WithMany(u => u.usuarios)
                .HasForeignKey(u => u.IdRol);
            builder.HasOne(p => p.personales)
                .WithMany(u => u.usuarios)
                .HasForeignKey(u => u.IdPersonal);
            builder.Property(u => u.Usuario)
                .HasMaxLength(30);

        }
    }
}
