using System;

namespace GenealogyApp.Domain.Entities;

public class Relationship
{

    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid FromMemberId { get; set; }
    public Guid ToMemberId { get; set; }
    public int TypeId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public FamilyMember FromMember { get; set; } = default!;
    public FamilyMember ToMember { get; set; } = default!;
    public RelationshipType Type { get; set; } = default!;

}
