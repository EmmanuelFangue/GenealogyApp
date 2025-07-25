
using GenealogyApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GenealogyApp.Infrastructure.Configurations
{
    public class FamilyLinkConfiguration : IEntityTypeConfiguration<FamilyLink>
    {
        public void Configure(EntityTypeBuilder<FamilyLink> builder)
        {
            builder.HasKey(f => f.LinkId);
            builder.Property(f => f.Status).HasMaxLength(20).IsRequired();
            builder.Property(f => f.RelationType).HasMaxLength(50);
            builder.Property(f => f.CreatedAt).HasDefaultValueSql("GETDATE()");

            builder.HasOne<User>()
                   .WithMany()
                   .HasForeignKey(f => f.RequesterId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<User>()
                   .WithMany()
                   .HasForeignKey(f => f.ReceiverId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
