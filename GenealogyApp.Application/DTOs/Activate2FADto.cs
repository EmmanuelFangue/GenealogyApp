namespace GenealogyApp.Application.DTOs
{
    public class Activate2FADto
    {
        public Guid UserId { get; set; }
        public string Code { get; set; } // Ex: code SMS ou email
    }
}