using GenealogyApp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GenealogyApp.API.Endpoints;

public static class TreeEndpoints
{
    public static RouteGroupBuilder MapTreeEndpoints(this RouteGroupBuilder g)
    {
        var tree = g.MapGroup("/tree");

        tree.MapGet("/{memberId:guid}/ancestors", async ([FromServices] IGenealogyService svc, Guid memberId, CancellationToken ct) =>
        {
            var list = await svc.GetAncestorsAsync(memberId, ct);
            return Results.Ok(list);
        });

        tree.MapGet("/common-ancestor", async ([FromServices] IGenealogyService svc, Guid a, Guid b, CancellationToken ct) =>
        {
            var res = await svc.GetClosestCommonAncestorAsync(a, b, ct);
            return res is null ? Results.NotFound() : Results.Ok(res);
        });

        return g;
    }
}
