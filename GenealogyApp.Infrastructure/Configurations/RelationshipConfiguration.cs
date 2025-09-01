using GenealogyApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GenealogyApp.Infrastructure.Configurations;

public class RelationshipConfiguration : IEntityTypeConfiguration<Relationship>
{
    public void Configure(EntityTypeBuilder<Relationship> b)
    {
        b.ToTable("Relationship", schema: "genea");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).ValueGeneratedOnAdd();

        b.Property(x => x.TypeId).IsRequired();
        b.HasIndex(x => new { x.FromMemberId, x.ToMemberId, x.TypeId }).IsUnique();

        b.HasIndex(x => x.FromMemberId);
        b.HasIndex(x => x.ToMemberId);

        // Relations optionnelles (dÃ©commente si tu veux les navigations)
        // b.HasOne<RelationshipType>().WithMany().HasForeignKey(x => x.TypeId);
        // b.HasOne<FamilyMember>().WithMany().HasForeignKey(x => x.FromMemberId).OnDelete(DeleteBehavior.Cascade);
        // b.HasOne<FamilyMember>().WithMany().HasForeignKey(x => x.ToMemberId).OnDelete(DeleteBehavior.Cascade);
    }
}
