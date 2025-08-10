using GenealogyApp.Application.Interfaces;
using GenealogyApp.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace GenealogyApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AuditLogController : ControllerBase
    {
        private readonly IAuditService _service;

        public AuditLogController(IAuditService service)
        {
            _service = service;
        }

        // Lister tous les logs (avec filtres possibles)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuditLogDto>>> GetLogs([FromQuery] AuditLogSearchDto search)
        {
            var logs = await _service.GetLogsAsync(search);
            return Ok(logs);
        }

        // Récupérer les logs d’une entité spécifique
        [HttpGet("entity/{entity}/{entityId}")]
        public async Task<ActionResult<IEnumerable<AuditLogDto>>> GetLogsForEntity(string entity, Guid entityId)
        {
            var logs = await _service.GetLogsForEntityAsync(entity, entityId);
            return Ok(logs);
        }

        // Récupérer les logs pour un utilisateur
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<AuditLogDto>>> GetLogsForUser(Guid userId)
        {
            var logs = await _service.GetLogsForUserAsync(userId);
            return Ok(logs);
        }
    }
}