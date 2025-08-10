using System;
using System.Collections.Generic;

namespace GenealogyApp.Domain.Entities
{
    public class FamilyMember
    {
        public Guid MemberId { get; set; }
        public Guid UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Gender { get; set; }
        public string? RelationToUser { get; set; }
        public string? ProfilePhotoUrl { get; set; }
        public string? Summary { get; set; }


        public User? User { get; set; }
        public ICollection<Photo>? Photos { get; set; }
    }
}