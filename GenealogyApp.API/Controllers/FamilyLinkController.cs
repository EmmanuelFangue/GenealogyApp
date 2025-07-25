using GenealogyApp.Application.Interfaces;
using GenealogyApp.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GenealogyApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FamilyLinkController : ControllerBase
    {
        private readonly ILinkService _service;

        public FamilyLinkController(ILinkService service)
        {
            _service = service;
        }

        [HttpPost("request")]
        public async Task<IActionResult> RequestLink(FamilyLink link)
        {
            await _service.SendConnectionRequestAsync(link);
            return Ok();
        }

        [HttpPost("accept/{linkId}")]
        public async Task<IActionResult> Accept(Guid linkId)
        {
            await _service.AcceptConnectionAsync(linkId);
            return Ok();
        }

        [HttpPost("reject/{linkId}")]
        public async Task<IActionResult> Reject(Guid linkId)
        {
            await _service.RejectConnectionAsync(linkId);
            return Ok();
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<FamilyLink>>> GetLinks(Guid userId)
        {
            var links = await _service.GetLinksForUserAsync(userId);
            return Ok(links);
        }
    }
}