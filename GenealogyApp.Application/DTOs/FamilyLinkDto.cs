
namespace GenealogyApp.Application.DTOs
{
    public class FamilyLinkDto
    {
        public Guid LinkId { get; set; }
        public Guid RequesterId { get; set; }
        public Guid ReceiverId { get; set; }
        public string Status { get; set; }
        public string RelationType { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ConfirmedAt { get; set; }
    }
}
