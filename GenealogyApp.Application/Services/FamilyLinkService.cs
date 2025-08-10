using GenealogyApp.Application.DTOs;
using GenealogyApp.Application.Interfaces;
using GenealogyApp.Domain.Entities;
using GenealogyApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GenealogyApp.Application.Services
{
    public class FamilyLinkService : IFamilyLinkService
    {
        private readonly GenealogyDbContext _db;

        public FamilyLinkService(GenealogyDbContext db)
        {
            _db = db;
        }

        public async Task<FamilyLinkDto?> RequestFamilyLinkAsync(FamilyLinkRequestDto request)
        {
            // Vérifier l'existence des utilisateurs et qu'il n'y a pas déjà une demande en cours
            var alreadyExists = await _db.FamilyLinks.AnyAsync(f =>
                f.RequesterId == request.RequesterId &&
                f.ReceiverId == request.ReceiverId &&
                f.Status == "Pending");

            if (alreadyExists) return null;

            var link = new FamilyLink
            {
                RequesterId = request.RequesterId,
                ReceiverId = request.ReceiverId,
                RelationType = request.RelationType,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow
            };

            _db.FamilyLinks.Add(link);
            await _db.SaveChangesAsync();

            return ToDto(link);
        }

        public async Task<FamilyLinkDto?> AcceptFamilyLinkAsync(Guid linkId)
        {
            var link = await _db.FamilyLinks.FirstOrDefaultAsync(l => l.LinkId == linkId);
            if (link == null || link.Status != "Pending") return null;

            link.Status = "Accepted";
            link.ConfirmedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();

            // Optionnel: ici tu peux proposer une suggestion automatique de RelationType si besoin

            return ToDto(link);
        }

        public async Task<bool> RejectFamilyLinkAsync(Guid linkId)
        {
            var link = await _db.FamilyLinks.FirstOrDefaultAsync(l => l.LinkId == linkId);
            if (link == null || link.Status != "Pending") return false;

            link.Status = "Rejected";
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CancelFamilyLinkAsync(Guid linkId)
        {
            var link = await _db.FamilyLinks.FirstOrDefaultAsync(l => l.LinkId == linkId);
            if (link == null || link.Status != "Pending") return false;

            link.Status = "Cancelled";
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<FamilyLinkDto>> GetUserLinksAsync(Guid userId)
        {
            var links = await _db.FamilyLinks
                .Where(l => l.RequesterId == userId || l.ReceiverId == userId)
                .OrderByDescending(l => l.CreatedAt)
                .Select(l => ToDto(l))
                .ToListAsync();

            return links;
        }

        private FamilyLinkDto ToDto(FamilyLink link) => new FamilyLinkDto
        {
            LinkId = link.LinkId,
            RequesterId = link.RequesterId,
            ReceiverId = link.ReceiverId,
            RelationType = link.RelationType,
            Status = link.Status,
            CreatedAt = link.CreatedAt,
            ConfirmedAt = link.ConfirmedAt
        };
    }
}