using GenealogyApp.Application.DTOs;

namespace GenealogyApp.Application.Interfaces
{
    public interface IFamilyLinkService
    {
        Task<FamilyLinkDto?> RequestFamilyLinkAsync(FamilyLinkRequestDto request);
        Task<FamilyLinkDto?> AcceptFamilyLinkAsync(Guid linkId);
        Task<bool> RejectFamilyLinkAsync(Guid linkId);
        Task<bool> CancelFamilyLinkAsync(Guid linkId);
        Task<IEnumerable<FamilyLinkDto>> GetUserLinksAsync(Guid userId);
    }
}