using Sistema.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sistema.Infrastructure.Data.Configuration
{
    public class DocumentosConfiguration : IEntityTypeConfiguration<Documentos>
    {
        public void Configure(EntityTypeBuilder<Documentos> builder)
        {
            builder.ToTable("Documentos")
                .HasKey(d => d.IdDocumento);
            builder.HasOne(d => d.usuario)
                .WithMany(u => u.documentos)
                .HasForeignKey(d => d.IdUsuario);
            builder.HasOne(d => d.personal)
                .WithMany(p => p.documentos)
                .HasForeignKey(d => d.IdPersonal);
            builder.HasOne(d => d.seccion)
                .WithMany(s => s.documentos)
                .HasForeignKey(d => d.IdSeccion);
        }
    }
}
