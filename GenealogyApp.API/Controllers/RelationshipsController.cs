using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using GenealogyApp.API.Validators;
using GenealogyApp.Application.DTOs;
using GenealogyApp.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GenealogyApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RelationshipsController : ControllerBase
{
    private readonly IGenealogyService _service;

    public RelationshipsController(IGenealogyService service)
    {
        _service = service;
    }

    [HttpPost("parent-of")]
    public async Task<IActionResult> AddParentOf([FromBody] AddParentOfRequest request, CancellationToken ct)
    {
        var error = AddParentOfRequestValidator.Validate(request);
        if (error is not null) return BadRequest(error);

        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        Guid.TryParse(userIdStr, out var userId);

        await _service.AddParentOfAsync(request.ParentId, request.ChildId, userId, ct);
        return NoContent();
    }
}
