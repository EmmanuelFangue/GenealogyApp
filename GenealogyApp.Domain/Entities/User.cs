using GenealogyApp.Domain.Entities;
using System;
using System.Collections.Generic;

namespace GenealogyApp.Domain.Entities
{
    public class User
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string PasswordHash { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<FamilyMember> FamilyMembers { get; set; } = new List<FamilyMember>();
        public ICollection<FamilyLink> SentLinks { get; set; }
        public ICollection<FamilyLink> ReceivedLinks { get; set; }
        public ICollection<AuditLog> AuditLogs { get; set; }
    }
}

