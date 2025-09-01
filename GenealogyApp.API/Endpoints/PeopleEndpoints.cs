using GenealogyApp.Infrastructure.Data;
using GenealogyApp.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GenealogyApp.API.Endpoints;

public static class PeopleEndpoints
{
    public record PersonCreateDto(string FirstName, string? LastName, DateTime? BirthDate, string? Gender, string? RelationToUser, string? Summary);
    public record PersonUpdateDto(string? FirstName, string? LastName, DateTime? BirthDate, string? Gender, string? RelationToUser, string? Summary);

    public static RouteGroupBuilder MapPeopleEndpoints(this RouteGroupBuilder g)
    {
        var people = g.MapGroup("/people");

        people.MapPost("/", async (
            [FromServices] GenealogyDbContext db,
            ClaimsPrincipal user,
            [FromBody] PersonCreateDto dto) =>
        {
            var userId = Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var m = new FamilyMember
            {
                MemberId = Guid.NewGuid(),
                UserId = userId,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                BirthDate = dto.BirthDate,
                Gender = dto.Gender,
                RelationToUser = dto.RelationToUser,
                Summary = dto.Summary
            };

            db.FamilyMembers.Add(m);
            await db.SaveChangesAsync();
            return Results.Created($"/api/people/{m.MemberId}", m);
        });

        people.MapGet("/{id:guid}", async ([FromServices] GenealogyDbContext db, Guid id) =>
        {
            var m = await db.FamilyMembers.AsNoTracking().FirstOrDefaultAsync(x => x.MemberId == id);
            return m is null ? Results.NotFound() : Results.Ok(m);
        });

        people.MapGet("/", async ([FromServices] GenealogyDbContext db, string? q, int take = 50, int skip = 0) =>
        {
            IQueryable<FamilyMember> query = db.FamilyMembers.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(q))
            {
                q = q.Trim();
                query = query.Where(x => (x.FirstName + " " + x.LastName).Contains(q));
            }
            var list = await query.OrderBy(x => x.LastName).ThenBy(x => x.FirstName)
                                  .Skip(skip).Take(Math.Clamp(take, 1, 200)).ToListAsync();
            return Results.Ok(list);
        });

        people.MapPut("/{id:guid}", async ([FromServices] GenealogyDbContext db, Guid id, [FromBody] PersonUpdateDto dto) =>
        {
            var m = await db.FamilyMembers.FirstOrDefaultAsync(x => x.MemberId == id);
            if (m is null) return Results.NotFound();

            if (!string.IsNullOrWhiteSpace(dto.FirstName)) m.FirstName = dto.FirstName!;
            if (!string.IsNullOrWhiteSpace(dto.LastName)) m.LastName = dto.LastName!;
            if (dto.BirthDate is not null) m.BirthDate = dto.BirthDate;
            if (!string.IsNullOrWhiteSpace(dto.Gender)) m.Gender = dto.Gender!;
            if (!string.IsNullOrWhiteSpace(dto.RelationToUser)) m.RelationToUser = dto.RelationToUser!;
            if (!string.IsNullOrWhiteSpace(dto.Summary)) m.Summary = dto.Summary!;
            await db.SaveChangesAsync();
            return Results.NoContent();
        });

        people.MapDelete("/{id:guid}", async ([FromServices] GenealogyDbContext db, Guid id) =>
        {
            var m = await db.FamilyMembers.FirstOrDefaultAsync(x => x.MemberId == id);
            if (m is null) return Results.NotFound();
            db.FamilyMembers.Remove(m);
            await db.SaveChangesAsync();
            return Results.NoContent();
        });

        return g;
    }
}
