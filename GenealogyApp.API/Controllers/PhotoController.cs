using GenealogyApp.Application.Interfaces;
using GenealogyApp.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace GenealogyApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PhotoController : ControllerBase
    {
        private readonly IPhotoService _photoService;

        public PhotoController(IPhotoService photoService)
        {
            _photoService = photoService;
        }

        /// <summary>
        /// Ajoute une photo à un membre de la famille (max 20).
        /// </summary>
        [HttpPost("add")]
        [ProducesResponseType(typeof(PhotoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PhotoDto>> AddPhoto([FromForm] PhotoUploadDto dto)
        {
            if (dto.File == null || dto.File.Length == 0)
                return BadRequest("Aucun fichier fourni.");

            var result = await _photoService.AddPhotoAsync(dto);

            if (result == null)
                return BadRequest("Limite de 20 photos atteinte ou erreur technique.");

            return Ok(result);
        }




        /// <summary>
        /// Liste les photos d'un membre.
        /// </summary>
        [HttpGet("{memberId}")]
        public async Task<ActionResult<IEnumerable<PhotoDto>>> GetPhotos(Guid memberId)
        {
            var photos = await _photoService.GetPhotosAsync(memberId);
            return Ok(photos);
        }

        /// <summary>
        /// Supprime une photo d'un membre.
        /// </summary>
        [HttpDelete("{memberId}/{photoId}")]
        public async Task<IActionResult> DeletePhoto(Guid memberId, Guid photoId)
        {
            var success = await _photoService.DeletePhotoAsync(memberId, photoId);
            return success ? NoContent() : NotFound("Photo non trouvée ou membre non valide.");
        }

        [HttpPut("{memberId}/set-profile/{photoId}")]
        public async Task<IActionResult> SetProfilePhoto(Guid memberId, Guid photoId)
        {
            var success = await _photoService.SetProfilePhotoAsync(memberId, photoId);
            return success ? NoContent() : NotFound("Photo ou membre introuvable.");
        }

    }
}