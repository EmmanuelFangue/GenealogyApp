using GenealogyApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

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
            // Applique les configurations Fluent API
            modelBuilder.ApplyConfiguration(new Configurations.UserConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.FamilyMemberConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.PhotoConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.FamilyLinkConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.AuditLogConfiguration());

            // Appelle le OnModelCreating de la base
            base.OnModelCreating(modelBuilder);
        }
    }
}