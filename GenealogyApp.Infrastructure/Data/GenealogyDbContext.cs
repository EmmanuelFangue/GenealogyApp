using GenealogyApp.Domain.Entities;
using GenealogyApp.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace GenealogyApp.Infrastructure.Data
{
    public class GenealogyDbContext : DbContext
    {
        public GenealogyDbContext(DbContextOptions<GenealogyDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<FamilyMember> FamilyMembers { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<FamilyLink> FamilyLinks { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Exemple de configuration Fluent API
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<FamilyLink>()
                .Property(f => f.Status)
                .HasConversion<string>();

            modelBuilder.Entity<AuditLog>()
                .Property(a => a.Timestamp)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new FamilyMemberConfiguration());
            modelBuilder.ApplyConfiguration(new PhotoConfiguration());
            modelBuilder.ApplyConfiguration(new FamilyLinkConfiguration());
            modelBuilder.ApplyConfiguration(new AuditLogConfiguration());

        }
    }
}
