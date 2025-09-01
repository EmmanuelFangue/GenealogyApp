using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GenealogyApp.Application.DTOs;
using GenealogyApp.Application.Interfaces;
using GenealogyApp.Domain.Entities;
using GenealogyApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GenealogyApp.Infrastructure.Services;

public class GenealogyService : IGenealogyService
{
    private readonly GenealogyDbContext _db;

    public GenealogyService(GenealogyDbContext db)
    {
        _db = db;
    }

    public async Task AddParentOfAsync(Guid parentId, Guid childId, Guid performedByUserId, CancellationToken ct = default)
    {
        if (parentId == Guid.Empty || childId == Guid.Empty)
            throw new ArgumentException("ParentId et ChildId sont requis.");
        if (parentId == childId)
            throw new ArgumentException("Un membre ne peut pas Ãªtre son propre parent.");

        var exists = await _db.FamilyMembers
            .Where(m => m.MemberId == parentId || m.MemberId == childId)
            .Select(m => m.MemberId)
            .ToListAsync(ct);

        if (!exists.Contains(parentId) || !exists.Contains(childId))
            throw new InvalidOperationException("ParentId ou ChildId introuvable.");

        await _db.Database.ExecuteSqlInterpolatedAsync(
            $"EXEC genea.AddParentOf {parentId}, {childId}", ct);

        _db.AuditLogs.Add(new AuditLog
        {
            UserId = performedByUserId,
            Action = "AddParentOf",
            Entity = "Relationship",
            EntityId = Guid.Empty,
            Timestamp = DateTime.UtcNow
        });
        await _db.SaveChangesAsync(ct);
    }

    public async Task<IReadOnlyList<AncestorDto>> GetAncestorsAsync(Guid memberId, CancellationToken ct = default)
    {
        var query =
            from ac in _db.AncestorClosures.AsNoTracking()
            join p in _db.FamilyMembers.AsNoTracking() on ac.AncestorId equals p.MemberId
            where ac.DescendantId == memberId
            orderby ac.Depth
            select new AncestorDto(p.MemberId, p.FirstName, p.LastName, ac.Depth);

        return await query.ToListAsync(ct);
    }

    public async Task<CommonAncestorDto?> GetClosestCommonAncestorAsync(Guid a, Guid b, CancellationToken ct = default)
    {
        var query =
            from ac1 in _db.AncestorClosures.AsNoTracking()
            join ac2 in _db.AncestorClosures.AsNoTracking()
                on ac1.AncestorId equals ac2.AncestorId
            where ac1.DescendantId == a && ac2.DescendantId == b
            select new { ac1.AncestorId, DepthA = ac1.Depth, DepthB = ac2.Depth, TotalDepth = ac1.Depth + ac2.Depth };

        var best = await query.OrderBy(x => x.TotalDepth).FirstOrDefaultAsync(ct);
        if (best is null) return null;

        var person = await _db.FamilyMembers.AsNoTracking()
                         .Where(m => m.MemberId == best.AncestorId)
                         .Select(m => new { m.MemberId, m.FirstName, m.LastName })
                         .FirstOrDefaultAsync(ct);

        return new CommonAncestorDto(
            best.AncestorId,
            person?.FirstName,
            person?.LastName,
            best.DepthA,
            best.DepthB,
            best.TotalDepth
        );
    }
}
