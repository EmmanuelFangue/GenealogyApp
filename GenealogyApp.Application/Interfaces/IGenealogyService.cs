using GenealogyApp.Application.DTOs;

namespace GenealogyApp.Application.Interfaces
{
    public interface IGenealogyService 
    {
        // ... autres méthodes ...
        Task AddParentOfAsync(Guid parentId, Guid childId, CancellationToken ct);
        // appelle la proc stockée genea.AddParentOf (préserve la closure)

        Task<IReadOnlyList<AncestorDto>> GetAncestorsAsync(Guid memberId, CancellationToken ct);
        // SELECT ac join FamilyMembers ORDER BY Depth

        Task<CommonAncestorDto?> GetClosestCommonAncestorAsync(Guid a, Guid b, CancellationToken ct);
        // TOP(1) ancêtre commun par somme des profondeurs

    }
}