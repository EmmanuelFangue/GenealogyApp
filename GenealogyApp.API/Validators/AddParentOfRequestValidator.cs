using System;
using GenealogyApp.Application.DTOs;

namespace GenealogyApp.API.Validators;

public static class AddParentOfRequestValidator
{
    public static string? Validate(AddParentOfRequest r)
    {
        if (r.ParentId == Guid.Empty) return "ParentId requis.";
        if (r.ChildId == Guid.Empty) return "ChildId requis.";
        if (r.ChildId == r.ParentId) return "ChildId ne peut pas Ãªtre Ã©gal Ã  ParentId.";
        return null;
    }
}
