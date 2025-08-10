
using Microsoft.AspNetCore.Http;

namespace GenealogyApp.Application.DTOs
{
    public class PhotoUploadDto
    {
        public Guid MemberId { get; set; }
        public IFormFile File { get; set; }
    }

}
