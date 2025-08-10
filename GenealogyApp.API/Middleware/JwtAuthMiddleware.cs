using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace GenealogyApp.API.Middleware
{
    public class JwtAuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _config;

        public JwtAuthMiddleware(RequestDelegate next, IConfiguration config)
        {
            _next = next;
            _config = config;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (!string.IsNullOrEmpty(token))
            {
                try
                {
                    var handler = new JwtSecurityTokenHandler();
                    var jwtToken = handler.ReadJwtToken(token);

                    // Vérification basique du token (signature, expiration)
                    // Pour une validation complète, utiliser IOptions<JwtBearerOptions> ou TokenValidationParameters
                    var exp = jwtToken.ValidTo;
                    if (exp < DateTime.UtcNow)
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsync("Token expiré");
                        return;
                    }

                    context.Items["UserId"] = jwtToken.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
                    // Ajoute d'autres claims si besoin
                }
                catch
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Token JWT invalide");
                    return;
                }
            }
            else
            {
                // Si le endpoint nécessite une authentification (Authorize), mais pas de token
                // Tu peux filtrer ici ou laisser le filtre d’attribut faire le boulot
            }

            await _next(context);
        }
    }
}