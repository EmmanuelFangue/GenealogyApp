using GenealogyApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.AccessControl;

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
        public DbSet<AncestorClosure> AncestorClosures { get; set; }
        public DbSet<Relationship> Relationships { get; set; }
        
        public DbSet<RelationshipType> RelationshipTypes { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Applique les configurations Fluent API
            modelBuilder.ApplyConfiguration(new Configurations.UserConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.FamilyMemberConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.PhotoConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.FamilyLinkConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.RelationshipTypeConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.AuditLogConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.AncestorClosureConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.RelationshipConfiguration());

            // Appelle le OnModelCreating de la base
            base.OnModelCreating(modelBuilder);
        }
    }
}

