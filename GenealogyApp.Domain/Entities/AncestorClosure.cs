using System;

namespace GenealogyApp.Domain.Entities
{
        
    public class AncestorClosure 
    {
        public Guid AncestorId { get; set; }
        public Guid DescendantId { get; set; }
        public int Depth { get; set; }
    }

}