namespace GenealogyApp.Domain.Entities;

public class RelationshipType
{
    public int Id { get; set; }
    public string Code { get; set; } = default!; // ex: "ParentOf", "MarriedTo", "SiblingOf"
}
