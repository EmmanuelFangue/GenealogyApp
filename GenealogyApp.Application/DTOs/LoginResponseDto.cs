namespace GenealogyApp.Application.DTOs
{
    public class LoginResponseDto
    {
        public Guid UserId { get; set; }
        public string Token { get; set; }
        public bool TwoFactorEnabled { get; set; }
    }
}