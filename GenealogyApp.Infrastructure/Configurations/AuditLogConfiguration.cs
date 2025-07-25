
using GenealogyApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GenealogyApp.Infrastructure.Configurations
{
    public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
    {
        public void Configure(EntityTypeBuilder<AuditLog> builder)
        {
            builder.HasKey(a => a.LogId);
            builder.Property(a => a.Action).HasMaxLength(100).IsRequired();
            builder.Property(a => a.Entity).HasMaxLength(100).IsRequired();
            builder.Property(a => a.Timestamp).HasDefaultValueSql("GETDATE()");
        }
    }
}
