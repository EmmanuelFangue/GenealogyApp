namespace GenealogyApp.Application.Interfaces
{
    public interface IUserService
    {
        Task<Guid> RegisterUserAsync(string username, string email, string password, string phoneNumber);
        Task<string> AuthenticateAsync(string username, string password);
        Task<bool> EnableTwoFactorAsync(Guid userId, bool enabled);
    }
}