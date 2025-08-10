using GenealogyApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GenealogyApp.Infrastructure.Configurations
{
    public class FamilyLinkConfiguration : IEntityTypeConfiguration<FamilyLink>
    {
        public void Configure(EntityTypeBuilder<FamilyLink> builder)
        {
            builder.ToTable("FamilyLinks", "genea");
            builder.HasKey(l => l.LinkId);

            builder.Property(l => l.Status)
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(l => l.RelationType)
                .HasMaxLength(50);

            builder.Property(l => l.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            builder.Property(l => l.ConfirmedAt);

            // Configuration explicite des deux relations vers User
            builder.HasOne(l => l.Requester)
                .WithMany() // ou .WithMany(u => u.RequestedLinks) si tu veux une navigation inverse
                .HasForeignKey(l => l.RequesterId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(l => l.Receiver)
                .WithMany() // ou .WithMany(u => u.ReceivedLinks)
                .HasForeignKey(l => l.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
