using System;

namespace GenealogyApp.Domain.Entities
{
    public class RelationshipType 
    {
        public int Id { get; set; }
        public string Code { get; set; } = default!;
    }
}