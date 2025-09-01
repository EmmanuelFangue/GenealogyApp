using GenealogyApp.Application.DTOs;
using GenealogyApp.Application.Interfaces;
using GenealogyApp.Domain.Entities;
using GenealogyApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GenealogyApp.Infrastructure.Services
{
    public class AuditService : IAuditService
    {
        private readonly GenealogyDbContext _db;

        public AuditService(GenealogyDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<AuditLogDto>> GetLogsAsync(AuditLogSearchDto search)
        {
            var query = _db.AuditLogs.AsQueryable();

            if (search.UserId.HasValue)
                query = query.Where(l => l.UserId == search.UserId.Value);
            if (!string.IsNullOrWhiteSpace(search.Action))
                query = query.Where(l => l.Action.Contains(search.Action));
            if (!string.IsNullOrWhiteSpace(search.Entity))
                query = query.Where(l => l.Entity == search.Entity);
            if (search.EntityId.HasValue)
                query = query.Where(l => l.EntityId == search.EntityId.Value);
            if (search.From.HasValue)
                query = query.Where(l => l.Timestamp >= search.From.Value);
            if (search.To.HasValue)
                query = query.Where(l => l.Timestamp <= search.To.Value);

            return await query
                .OrderByDescending(l => l.Timestamp)
                .Select(l => new AuditLogDto
                {
                    LogId = l.LogId,
                    UserId = l.UserId,
                    Action = l.Action,
                    Entity = l.Entity,
                    EntityId = l.EntityId,
                    Timestamp = l.Timestamp
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<AuditLogDto>> GetLogsForEntityAsync(string entity, Guid entityId)
        {
            return await _db.AuditLogs
                .Where(l => l.Entity == entity && l.EntityId == entityId)
                .OrderByDescending(l => l.Timestamp)
                .Select(l => new AuditLogDto
                {
                    LogId = l.LogId,
                    UserId = l.UserId,
                    Action = l.Action,
                    Entity = l.Entity,
                    EntityId = l.EntityId,
                    Timestamp = l.Timestamp
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<AuditLogDto>> GetLogsForUserAsync(Guid userId)
        {
            return await _db.AuditLogs
                .Where(l => l.UserId == userId)
                .OrderByDescending(l => l.Timestamp)
                .Select(l => new AuditLogDto
                {
                    LogId = l.LogId,
                    UserId = l.UserId,
                    Action = l.Action,
                    Entity = l.Entity,
                    EntityId = l.EntityId,
                    Timestamp = l.Timestamp
                })
                .ToListAsync();
        }

        public async Task AddLogAsync(Guid userId, string action, string entity, Guid entityId)
        {
            var log = new AuditLog
            {
                UserId = userId,
                Action = action,
                Entity = entity,
                EntityId = entityId,
                Timestamp = DateTime.UtcNow
            };
            _db.AuditLogs.Add(log);
            await _db.SaveChangesAsync();
        }
    }
}