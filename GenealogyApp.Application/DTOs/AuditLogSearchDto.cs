namespace GenealogyApp.Application.DTOs
{
    public class AuditLogSearchDto
    {
        public Guid? UserId { get; set; }
        public string? Action { get; set; }
        public string? Entity { get; set; }
        public Guid? EntityId { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}