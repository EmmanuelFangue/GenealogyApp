using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GenealogyApp.Application.DTOs;

namespace GenealogyApp.Application.Interfaces;

public interface IGenealogyService
{
    Task AddParentOfAsync(Guid parentId, Guid childId, Guid performedByUserId, CancellationToken ct = default);

    Task<IReadOnlyList<AncestorDto>> GetAncestorsAsync(Guid memberId, CancellationToken ct = default);

    Task<CommonAncestorDto?> GetClosestCommonAncestorAsync(Guid memberAId, Guid memberBId, CancellationToken ct = default);
}
