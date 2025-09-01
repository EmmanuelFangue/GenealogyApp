using System;

namespace GenealogyApp.Domain.Entities
{    
    public class Relationship
    {
        public Guid Id { get; set; }
        public Guid FromMemberId { get; set; }
        public Guid ToMemberId { get; set; }
        public int TypeId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

}