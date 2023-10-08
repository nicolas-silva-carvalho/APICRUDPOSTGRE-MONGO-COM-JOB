using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using teste.model;

namespace teste.db.Configure
{
    public class ConfigMap : IEntityTypeConfiguration<BookContrato>
    {
        public void Configure(EntityTypeBuilder<BookContrato> builder)
        {
            builder.ToTable("BookContratos");

            // Chave primária
            builder.HasKey(bc => bc.Id);

            // Configuração da propriedade Erro
            builder.Property(bc => bc.Erro)
                .IsRequired();

            // Configuração da propriedade DateTime
            builder.Property(bc => bc.DateTime)
                .IsRequired();

            // Configuração da propriedade IdBook como chave estrangeira
            builder.Property(bc => bc.IdBook)
                .IsRequired();

            // Configuração da relação com a classe Book
            builder.HasOne(bc => bc.Book)
                .WithMany()
                .HasForeignKey(bc => bc.IdBook)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}