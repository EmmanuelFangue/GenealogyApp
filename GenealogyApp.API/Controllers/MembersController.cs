using System;
using System.Threading;
using System.Threading.Tasks;
using GenealogyApp.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GenealogyApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MembersController : ControllerBase
{
    private readonly IGenealogyService _service;

    public MembersController(IGenealogyService service)
    {
        _service = service;
    }

    [HttpGet("{memberId:guid}/ancestors")]
    public async Task<IActionResult> GetAncestors([FromRoute] Guid memberId, CancellationToken ct)
    {
        var list = await _service.GetAncestorsAsync(memberId, ct);
        return Ok(list);
    }

    [HttpGet("common-ancestor")]
    public async Task<IActionResult> GetClosestCommonAncestor([FromQuery] Guid a, [FromQuery] Guid b, CancellationToken ct)
    {
        if (a == Guid.Empty || b == Guid.Empty) return BadRequest("ParamÃ¨tres a et b requis.");
        var result = await _service.GetClosestCommonAncestorAsync(a, b, ct);
        return result is null ? NotFound() : Ok(result);
    }
}
