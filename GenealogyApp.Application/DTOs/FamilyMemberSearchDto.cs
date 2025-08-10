namespace GenealogyApp.Application.DTOs
{
    public class FamilyMemberSearchDto
    {
        public Guid? UserId { get; set; } // Pour lister les membres d'un utilisateur
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? RelationToUser { get; set; }
        public DateTime? MinBirthDate { get; set; }
        public DateTime? MaxBirthDate { get; set; }
        public string? Gender { get; set; }
    }
}