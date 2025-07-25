using GenealogyApp.Application.Interfaces;
using GenealogyApp.Domain.Entities;
using GenealogyApp.Infrastructure.Data;

namespace GenealogyApp.Application.Services
{
    public class AuditService : IAuditService
    {
        private readonly GenealogyDbContext _context;

        public AuditService(GenealogyDbContext context)
        {
            _context = context;
        }

        public async Task LogActionAsync(Guid userId, string action, string entity, Guid entityId)
        {
            var log = new AuditLog
            {
                UserId = userId,
                Action = action,
                Entity = entity,
                EntityId = entityId,
                Timestamp = DateTime.UtcNow
            };

            _context.AuditLogs.Add(log);
            await _context.SaveChangesAsync();
        }
    }
}
