using GenealogyApp.Application.DTOs;
using GenealogyApp.Application.Interfaces;
using GenealogyApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GenealogyApp.Infrastructure.Services
{
    public class FamilyService : IFamilyService
    {
        private readonly GenealogyDbContext _db;

        public FamilyService(GenealogyDbContext db)
        {
            _db = db;
        }

        // ... autres méthodes ...

        public async Task<IEnumerable<FamilyMemberDto>> SearchMembersAsync(FamilyMemberSearchDto criteria)
        {
            var query = _db.FamilyMembers.AsQueryable();

            if (criteria.UserId.HasValue)
                query = query.Where(m => m.UserId == criteria.UserId.Value);

            if (!string.IsNullOrWhiteSpace(criteria.FirstName))
                query = query.Where(m => m.FirstName.Contains(criteria.FirstName));

            if (!string.IsNullOrWhiteSpace(criteria.LastName))
                query = query.Where(m => m.LastName != null && m.LastName.Contains(criteria.LastName));

            if (!string.IsNullOrWhiteSpace(criteria.RelationToUser))
                query = query.Where(m => m.RelationToUser != null && m.RelationToUser.Contains(criteria.RelationToUser));

            if (criteria.MinBirthDate.HasValue)
                query = query.Where(m => m.BirthDate != null && m.BirthDate >= criteria.MinBirthDate.Value);

            if (criteria.MaxBirthDate.HasValue)
                query = query.Where(m => m.BirthDate != null && m.BirthDate <= criteria.MaxBirthDate.Value);

            if (!string.IsNullOrWhiteSpace(criteria.Gender))
                query = query.Where(m => m.Gender == criteria.Gender);

            var members = await query.Select(m => new FamilyMemberDto
            {
                MemberId = m.MemberId,
                UserId = m.UserId,
                FirstName = m.FirstName,
                LastName = m.LastName,
                BirthDate = (DateTime)m.BirthDate,
                Gender = m.Gender,
                RelationToUser = m.RelationToUser,
                ProfilePhotoUrl = m.ProfilePhotoUrl,
                Summary = m.Summary
            }).ToListAsync();

            return members;
        }
    }
}