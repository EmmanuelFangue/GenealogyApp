namespace GenealogyApp.Application.Interfaces
{
    public interface IFamilyService
    {
        Task<Guid> AddFamilyMemberAsync(Guid userId, string firstName, string lastName, DateTime birthDate, string gender, string relationToUser);
        Task<bool> UpdateFamilyMemberAsync(Guid memberId, string firstName, string lastName, DateTime birthDate, string gender, string relationToUser);
        Task<bool> RemoveFamilyMemberAsync(Guid memberId);
    }
}