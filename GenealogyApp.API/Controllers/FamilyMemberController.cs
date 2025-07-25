using GenealogyApp.Application.Interfaces;
using GenealogyApp.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GenealogyApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FamilyMemberController : ControllerBase
    {
        private readonly IFamilyService _service;

        public FamilyMemberController(IFamilyService service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FamilyMember>> Get(Guid id)
        {
            var member = await _service.GetMemberByIdAsync(id);
            return member == null ? NotFound() : Ok(member);
        }

        [HttpPost]
        public async Task<ActionResult<FamilyMember>> Create(FamilyMember member)
        {
            var created = await _service.AddFamilyMemberAsync(member);
            return CreatedAtAction(nameof(Get), new { id = created.MemberId }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, FamilyMember member)
        {
            if (id != member.MemberId) return BadRequest();
            await _service.UpdateFamilyMemberAsync(member);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.RemoveFamilyMemberAsync(id);
            return NoContent();
        }
    }
}