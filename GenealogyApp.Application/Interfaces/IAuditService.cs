using GenealogyApp.Application.DTOs;

namespace GenealogyApp.Application.Interfaces
{
    public interface IAuditService
    {
        Task<IEnumerable<AuditLogDto>> GetLogsAsync(AuditLogSearchDto search);
        Task<IEnumerable<AuditLogDto>> GetLogsForEntityAsync(string entity, Guid entityId);
        Task<IEnumerable<AuditLogDto>> GetLogsForUserAsync(Guid userId);
        Task AddLogAsync(Guid userId, string action, string entity, Guid entityId);
    }
}