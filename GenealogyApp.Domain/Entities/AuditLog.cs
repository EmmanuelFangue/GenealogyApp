using System;

namespace GenealogyApp.Domain.Entities
{
    public class AuditLog
    {
        public int LogId { get; set; }
        public Guid UserId { get; set; }
        public string ?Action { get; set; }
        public string ?Entity { get; set; }
        public Guid EntityId { get; set; }
        public DateTime Timestamp { get; set; }

        public User ?User { get; set; }
    }
}