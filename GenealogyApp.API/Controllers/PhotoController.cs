using GenealogyApp.Application.Interfaces;
using GenealogyApp.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GenealogyApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PhotoController : ControllerBase
    {
        private readonly IFamilyService _service;

        public PhotoController(IFamilyService service)
        {
            _service = service;
        }

        [HttpGet("{memberId}")]
        public async Task<ActionResult<IEnumerable<Photo>>> GetPhotos(Guid memberId)
        {
            var photos = await _service.GetPhotosByMemberIdAsync(memberId);
            return Ok(photos);
        }

        [HttpPost("{memberId}")]
        public async Task<IActionResult> Upload(Guid memberId, List<IFormFile> files)
        {
            await _service.UploadPhotosAsync(memberId, files);
            return Ok();
        }

        [HttpDelete("{photoId}")]
        public async Task<IActionResult> Delete(Guid photoId)
        {
            await _service.DeletePhotoAsync(photoId);
            return NoContent();
        }
    }
}