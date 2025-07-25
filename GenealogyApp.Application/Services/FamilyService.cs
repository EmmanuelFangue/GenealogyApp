using GenealogyApp.Application.Interfaces;
using GenealogyApp.Domain.Entities;
using GenealogyApp.Infrastructure.Data;

namespace GenealogyApp.Application.Services
{
    public class FamilyService : IFamilyService
    {
        private readonly GenealogyDbContext _context;

        public FamilyService(GenealogyDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> AddFamilyMemberAsync(Guid userId, string firstName, string lastName, DateTime birthDate, string gender, string relationToUser)
        {
            var member = new FamilyMember
            {
                MemberId = Guid.NewGuid(),
                UserId = userId,
                FirstName = firstName,
                LastName = lastName,
                BirthDate = birthDate,
                Gender = gender,
                RelationToUser = relationToUser
            };

            _context.FamilyMembers.Add(member);
            await _context.SaveChangesAsync();
            return member.MemberId;
        }

        public Task<bool> UpdateFamilyMemberAsync(Guid memberId, string firstName, string lastName, DateTime birthDate, string gender, string relationToUser)
        {
            // TODO: Implement update logic
            return Task.FromResult(true);
        }

        public Task<bool> RemoveFamilyMemberAsync(Guid memberId)
        {
            // TODO: Implement remove logic
            return Task.FromResult(true);
        }
    }
}
}
