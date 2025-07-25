using System;

namespace GenealogyApp.Domain.Entities
{
    public class FamilyLink
    {
        public Guid LinkId { get; set; }
        public Guid RequesterId { get; set; }
        public Guid ReceiverId { get; set; }
        public string Status { get; set; }
        public string RelationType { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ConfirmedAt { get; set; }

        public User Requester { get; set; }
        public User Receiver { get; set; }
    }
}