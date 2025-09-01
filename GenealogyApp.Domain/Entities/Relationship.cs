using System;

namespace GenealogyApp.Domain.Entities;

public class Relationship
{
    public Guid Id { get; set; }           // PK (GUID)
    public Guid FromMemberId { get; set; } // source
    public Guid ToMemberId { get; set; }   // cible
    public int TypeId { get; set; }        // FK -> RelationshipType
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    // Props navigation (optionnelles)
    // public FamilyMember? FromMember { get; set; }
    // public FamilyMember? ToMember { get; set; }
    // public RelationshipType? Type { get; set; }
}
