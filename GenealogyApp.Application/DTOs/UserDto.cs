
namespace GenealogyApp.Application.DTOs
{
    public class UserDto
    {
        public Guid UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public bool TwoFactorEnabled { get; set; }

        // Nouveau champ
        public Guid? ProfileMemberId { get; set; }
        public DateTime CreatedAt { get; internal set; }
    }

}
