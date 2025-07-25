
namespace GenealogyApp.Application.DTOs
{
    public class AuditLogDto
    {
        public int LogId { get; set; }
        public Guid UserId { get; set; }
        public string Action { get; set; }
        public string Entity { get; set; }
        public Guid EntityId { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
