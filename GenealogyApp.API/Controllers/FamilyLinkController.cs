using GenealogyApp.Application.Interfaces;
using GenealogyApp.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace GenealogyApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FamilyLinkController : ControllerBase
    {
        private readonly IFamilyLinkService _linkService;

        public FamilyLinkController(IFamilyLinkService linkService)
        {
            _linkService = linkService;
        }

        // Demander une connexion familiale
        [HttpPost("request")]
        public async Task<ActionResult<FamilyLinkDto>> RequestFamilyLink([FromBody] FamilyLinkRequestDto request)
        {
            var result = await _linkService.RequestFamilyLinkAsync(request);
            return result != null ? Ok(result) : BadRequest("Impossible de créer la demande.");
        }

        // Accepter une demande de connexion familiale
        [HttpPost("{linkId}/accept")]
        public async Task<ActionResult<FamilyLinkDto>> AcceptFamilyLink(Guid linkId)
        {
            var result = await _linkService.AcceptFamilyLinkAsync(linkId);
            return result != null ? Ok(result) : NotFound("Demande non trouvée.");
        }

        // Refuser une demande de connexion familiale
        [HttpPost("{linkId}/reject")]
        public async Task<IActionResult> RejectFamilyLink(Guid linkId)
        {
            var success = await _linkService.RejectFamilyLinkAsync(linkId);
            return success ? NoContent() : NotFound("Demande non trouvée.");
        }

        // Annuler une demande de connexion familiale (par le demandeur)
        [HttpPost("{linkId}/cancel")]
        public async Task<IActionResult> CancelFamilyLink(Guid linkId)
        {
            var success = await _linkService.CancelFamilyLinkAsync(linkId);
            return success ? NoContent() : NotFound("Demande non trouvée.");
        }

        // Lister les demandes reçues ou envoyées
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<FamilyLinkDto>>> GetUserLinks(Guid userId)
        {
            var links = await _linkService.GetUserLinksAsync(userId);
            return Ok(links);
        }
    }
}