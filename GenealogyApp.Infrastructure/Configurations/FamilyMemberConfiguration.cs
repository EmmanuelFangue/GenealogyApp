
using GenealogyApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GenealogyApp.Infrastructure.Configurations
{
    public class FamilyMemberConfiguration : IEntityTypeConfiguration<FamilyMember>
    {
        public void Configure(EntityTypeBuilder<FamilyMember> builder)
        {
            builder.HasKey(f => f.MemberId);
            builder.Property(f => f.FirstName).HasMaxLength(100).IsRequired();
            builder.Property(f => f.LastName).HasMaxLength(100);
            builder.Property(f => f.Gender).HasMaxLength(10);
            builder.Property(f => f.RelationToUser).HasMaxLength(50);
            builder.Property(f => f.ProfilePhotoUrl).HasMaxLength(500);
            builder.Property(f => f.Summary).HasMaxLength(4000);

            builder.HasOne<User>()
                   .WithMany()
                   .HasForeignKey(f => f.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
