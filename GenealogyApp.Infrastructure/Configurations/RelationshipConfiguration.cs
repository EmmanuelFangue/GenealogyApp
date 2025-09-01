using GenealogyApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GenealogyApp.Infrastructure.Configurations;

public class RelationshipConfiguration : IEntityTypeConfiguration<Relationship>
{
    public void Configure(EntityTypeBuilder<Relationship> b)
    {
        b.ToTable("Relationship", "genea");
        b.HasKey(x => x.Id);
        b.HasOne(x => x.FromMember)
         .WithMany()
         .HasForeignKey(x => x.FromMemberId)
         .OnDelete(DeleteBehavior.Cascade);
        b.HasOne(x => x.ToMember)
         .WithMany()
         .HasForeignKey(x => x.ToMemberId)
         .OnDelete(DeleteBehavior.Cascade);
        b.HasIndex(x => new { x.FromMemberId, x.ToMemberId, x.TypeId }).IsUnique();
    }

}
