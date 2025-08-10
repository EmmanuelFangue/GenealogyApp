
namespace GenealogyApp.Application.DTOs
{
    public class PhotoDto
    {
        public Guid PhotoId { get; set; }
        public Guid MemberId { get; set; }
        public string Url { get; set; }
        public DateTime UploadedAt { get; set; }
        public string? Description { get; set; }
    }
}
