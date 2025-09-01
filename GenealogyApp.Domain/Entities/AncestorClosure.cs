using System;

namespace GenealogyApp.Domain.Entities;

public class AncestorClosure
{
    public Guid AncestorId { get; set; }
    public Guid DescendantId { get; set; }
    public int Depth { get; set; } // 1 = parent direct, 2 = grand-parent, etc.
}
