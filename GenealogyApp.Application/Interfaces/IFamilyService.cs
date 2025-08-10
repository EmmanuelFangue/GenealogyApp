using GenealogyApp.Application.DTOs;

namespace GenealogyApp.Application.Interfaces
{
    public interface IFamilyService
    {
        // ... autres m�thodes ...
        Task<IEnumerable<FamilyMemberDto>> SearchMembersAsync(FamilyMemberSearchDto criteria);
    }
}