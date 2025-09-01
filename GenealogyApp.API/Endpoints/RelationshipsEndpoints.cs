using GenealogyApp.Application.Interfaces;
using GenealogyApp.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GenealogyApp.API.Endpoints;

public static class RelationshipsEndpoints
{
    public record ParentOfDto(Guid ParentId, Guid ChildId);

    public static RouteGroupBuilder MapRelationshipsEndpoints(this RouteGroupBuilder g)
    {
        var rel = g.MapGroup("/relationships");

        // ParentOf via sproc genea.AddParentOf (closure maintenue côté SQL)
        rel.MapPost("/parent-of", async ([FromServices] IGenealogyService svc,
                                         [FromBody] ParentOfDto dto,
                                         HttpContext ctx) =>
        {
            var performedBy = ctx.User.GetUserId();
            await svc.AddParentOfAsync(dto.ParentId, dto.ChildId, performedBy, ctx.RequestAborted);
            return Results.NoContent();
        });

        // Exemple Spouse (edge direct côté EF)
        rel.MapPost("/spouse", async ([FromServices] GenealogyDbContext db,
                                      [FromBody] (Guid A, Guid B) body) =>
        {
            // On récupère TypeId pour 'MarriedTo'
            var typeId = await db.RelationshipTypes.Where(t => t.Code == "MarriedTo")
                                                   .Select(t => t.Id).FirstOrDefaultAsync();
            if (typeId == 0)
            {
                db.RelationshipTypes.Add(new Domain.Entities.RelationshipType { Code = "MarriedTo" });
                await db.SaveChangesAsync();
                typeId = await db.RelationshipTypes.Where(t => t.Code == "MarriedTo")
                                                   .Select(t => t.Id).FirstAsync();
            }

            // Crée les 2 sens A->B et B->A si absents (selon ton choix métier)
            async Task AddIfMissing(Guid from, Guid to)
            {
                var exists = await db.Relationships.AnyAsync(r => r.FromMemberId == from && r.ToMemberId == to && r.TypeId == typeId);
                if (!exists)
                    db.Relationships.Add(new Domain.Entities.Relationship
                    {
                        FromMemberId = from,
                        ToMemberId = to,
                        TypeId = typeId,
                        StartDate = DateTime.UtcNow
                    });
            }

            await AddIfMissing(body.A, body.B);
            await AddIfMissing(body.B, body.A);
            await db.SaveChangesAsync();
            return Results.NoContent();
        });

        return g;
    }

    private static Guid GetUserId(this System.Security.Claims.ClaimsPrincipal u)
        => Guid.Parse(u.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
}
