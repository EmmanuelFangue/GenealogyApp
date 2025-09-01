using GenealogyApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GenealogyApp.Infrastructure.Configurations;

public class AncestorClosureConfiguration : IEntityTypeConfiguration<AncestorClosure>
{
    public void Configure(EntityTypeBuilder<AncestorClosure> b)
    {
        b.ToTable("AncestorClosure", schema: "genea");
        b.HasKey(x => new { x.AncestorId, x.DescendantId });
        b.Property(x => x.Depth).IsRequired();

        b.HasIndex(x => x.DescendantId);
    }
}
