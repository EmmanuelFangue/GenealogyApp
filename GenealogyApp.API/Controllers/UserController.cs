using GenealogyApp.Application.Interfaces;
using GenealogyApp.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace GenealogyApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        // Inscription
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register([FromBody] RegisterUserDto dto)
        {
            var result = await _service.RegisterAsync(dto);
            return result != null ? Ok(result) : BadRequest("Inscription impossible.");
        }

        // Authentification
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginDto dto)
        {
            var result = await _service.LoginAsync(dto);
            return result != null ? Ok(result) : Unauthorized("Identifiants invalides.");
        }

        // Activation/SMS/2FA (exemple: code SMS)
        [HttpPost("activate")]
        public async Task<ActionResult<bool>> Activate2FA([FromBody] Activate2FADto dto)
        {
            var success = await _service.Activate2FAAsync(dto);
            return success ? Ok() : BadRequest("Activation 2FA impossible.");
        }

        // Récupération profil
        [HttpGet("{userId}")]
        [Authorize]
        public async Task<ActionResult<UserDto>> Get(Guid userId)
        {
            var user = await _service.GetUserAsync(userId);
            return user != null ? Ok(user) : NotFound();
        }

        // Mise à jour profil
        [HttpPut("{userId}")]
        [Authorize]
        public async Task<IActionResult> Update(Guid userId, [FromBody] UpdateUserDto dto)
        {
            var success = await _service.UpdateUserAsync(userId, dto);
            return success ? NoContent() : BadRequest();
        }

        // Suppression utilisateur
        [HttpDelete("{userId}")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid userId)
        {
            var success = await _service.DeleteUserAsync(userId);
            return success ? NoContent() : NotFound();
        }
    }
}
