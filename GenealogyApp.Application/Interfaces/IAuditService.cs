namespace GenealogyApp.Application.Interfaces
{
    public interface IAuditService
    {
        Task LogActionAsync(Guid userId, string action, string entity, Guid entityId);
    }
}