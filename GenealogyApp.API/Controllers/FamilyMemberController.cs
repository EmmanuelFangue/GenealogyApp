using GenealogyApp.Application.Interfaces;
using GenealogyApp.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace GenealogyApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FamilyMemberController : ControllerBase
    {
        private readonly IFamilyService _service;

        public FamilyMemberController(IFamilyService service)
        {
            _service = service;
        }

        // ... endpoints existants ...

        /// <summary>
        /// Recherche avancée de membres de la famille selon plusieurs critères.
        /// </summary>
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<FamilyMemberDto>>> Search([FromQuery] FamilyMemberSearchDto criteria)
        {
            var results = await _service.SearchMembersAsync(criteria);
            return Ok(results);
        }
    }
}