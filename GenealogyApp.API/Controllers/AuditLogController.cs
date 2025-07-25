using GenealogyApp.Application.Interfaces;
using GenealogyApp.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GenealogyApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuditLogController : ControllerBase
    {
        private readonly IAuditService _service;

        public AuditLogController(IAuditService service)
        {
            _service = service;
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<AuditLog>>> GetLogs(Guid userId)
        {
            var logs = await _service.GetLogsForUserAsync(userId);
            return Ok(logs);
        }
    }
}