using System;

namespace GenealogyApp.Domain.Entities
{
    public class Photo
    {
        public Guid PhotoId { get; set; }
        public Guid MemberId { get; set; }
        public string Url { get; set; }
        public DateTime UploadedAt { get; set; }

        public FamilyMember Member { get; set; }
    }
}