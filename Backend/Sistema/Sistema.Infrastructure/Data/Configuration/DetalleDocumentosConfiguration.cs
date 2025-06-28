using Sistema.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sistema.Infrastructure.Data.Configuration
{
    public class DetalleDocumentosConfiguration : IEntityTypeConfiguration<DetalleDocumentos>
    {
        public void Configure(EntityTypeBuilder<DetalleDocumentos> builder)
        {
            builder.ToTable("DetalleDocumentos")
                .HasKey(d => d.IdDetalleDocumentos);
            builder.HasOne(d => d.documento)
                .WithMany(o => o.detallesDocumentos)
                .HasForeignKey(d => d.IdDocumento);
            builder.HasOne(d => d.equipo)
                .WithMany(e => e.detallesDocumento)
                .HasForeignKey(d => d.IdEquipo);
        }
    }
}
