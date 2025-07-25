
namespace GenealogyApp.Application.DTOs
{
    public class FamilyMemberDto
    {
        public Guid MemberId { get; set; }
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }
        public string RelationToUser { get; set; }
        public string ProfilePhotoUrl { get; set; }
        public string Summary { get; set; }
    }
}
