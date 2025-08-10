using GenealogyApp.Application.DTOs;

namespace GenealogyApp.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserDto?> RegisterAsync(RegisterUserDto dto);
        Task<LoginResponseDto?> LoginAsync(LoginDto dto);
        Task<bool> Activate2FAAsync(Activate2FADto dto);
        Task<UserDto?> GetUserAsync(Guid userId);
        Task<bool> UpdateUserAsync(Guid userId, UpdateUserDto dto);
        Task<bool> DeleteUserAsync(Guid userId);
    }
}