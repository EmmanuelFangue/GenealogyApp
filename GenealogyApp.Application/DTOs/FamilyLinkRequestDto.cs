namespace GenealogyApp.Application.DTOs
{
    public class FamilyLinkRequestDto
    {
        public Guid RequesterId { get; set; }
        public Guid ReceiverId { get; set; }
        public string RelationType { get; set; } // Ex : "Cousin", "Frère", etc.
    }
}