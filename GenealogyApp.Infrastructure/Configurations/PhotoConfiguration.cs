using GenealogyApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GenealogyApp.Infrastructure.Configurations
{
    public class PhotoConfiguration : IEntityTypeConfiguration<Photo>
    {
        public void Configure(EntityTypeBuilder<Photo> builder)
        {
            builder.ToTable("Photos", "genea");
            builder.HasKey(p => p.PhotoId);

            builder.Property(p => p.Url)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(p => p.UploadedAt)
                .HasDefaultValueSql("GETDATE()");

            builder.HasOne<FamilyMember>()
                .WithMany()
                .HasForeignKey(p => p.MemberId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}