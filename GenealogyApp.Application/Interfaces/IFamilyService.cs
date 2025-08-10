using GenealogyApp.Application.DTOs;

namespace GenealogyApp.Application.Interfaces
{
    public interface IFamilyService
    {
        // ... autres méthodes ...
        Task<IEnumerable<FamilyMemberDto>> SearchMembersAsync(FamilyMemberSearchDto criteria);
    }
}