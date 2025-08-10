using GenealogyApp.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ProfileController : ControllerBase
{
    [Authorize]
    [HttpGet]
    public IActionResult GetProfile()
    {
        ProfileResponseDto profileResponse = new ProfileResponseDto
        {
            Id = User.FindFirst("sub")?.Value,
            Username = User.FindFirst("username")?.Value
        };
        return Ok(profileResponse);
    }
}