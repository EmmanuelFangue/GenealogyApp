using System;

namespace GenealogyApp.Application.DTOs;

public record AddParentOfRequest(Guid ParentId, Guid ChildId);

public record AncestorDto(
    Guid MemberId,
    string? FirstName,
    string? LastName,
    int Depth
);

public record CommonAncestorDto(
    Guid AncestorId,
    string? FirstName,
    string? LastName,
    int DepthA,
    int DepthB,
    int TotalDepth
);
