using GenealogyApp.Infrastructure.Data;
using GenealogyApp.Domain.Entities;
using GenealogyApp.Application.Interfaces;

namespace GenealogyApp.Application.Services
{
    public class UserService : IUserService
    {
        private readonly GenealogyDbContext _context;

        public UserService(GenealogyDbContext context)
        {
            _context = context;
        }

        // Exemple d'utilisation
        public async Task<Guid> RegisterUserAsync(string username, string email, string password, string phoneNumber)
        {
            var user = new User
            {
                UserId = Guid.NewGuid(),
                Username = username,
                Email = email,
                PhoneNumber = phoneNumber,
                PasswordHash = password, // à remplacer par un hash sécurisé
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user.UserId;
        }

        Task<string> IUserService.AuthenticateAsync(string username, string password)
        {
            throw new NotImplementedException();
        }

        Task<bool> IUserService.EnableTwoFactorAsync(Guid userId, bool enabled)
        {
            throw new NotImplementedException();
        }
    }
}
