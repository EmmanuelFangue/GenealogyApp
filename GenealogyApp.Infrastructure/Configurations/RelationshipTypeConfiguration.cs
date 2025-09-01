using GenealogyApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GenealogyApp.Infrastructure.Configurations;

public class RelationshipTypeConfiguration : IEntityTypeConfiguration<RelationshipType>
{
    public void Configure(EntityTypeBuilder<RelationshipType> b)
    {
        b.ToTable("RelationshipType", schema: "genea");
        b.HasKey(x => x.Id);
        b.Property(x => x.Code)
         .IsRequired()
         .HasMaxLength(50);
        b.HasIndex(x => x.Code).IsUnique();
    }
}
