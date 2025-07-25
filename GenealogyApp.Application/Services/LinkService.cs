using GenealogyApp.Application.Interfaces;
using GenealogyApp.Domain.Entities;
using GenealogyApp.Infrastructure.Data;

namespace GenealogyApp.Application.Services
{
    public class LinkService : ILinkService
    {
        private readonly GenealogyDbContext _context;

        public LinkService(GenealogyDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> SendConnectionRequestAsync(Guid requesterId, Guid receiverId, string relationType)
        {
            var link = new FamilyLink
            {
                LinkId = Guid.NewGuid(),
                RequesterId = requesterId,
                ReceiverId = receiverId,
                Status = "Pending",
                RelationType = relationType,
                CreatedAt = DateTime.UtcNow
            };

            _context.FamilyLinks.Add(link);
            await _context.SaveChangesAsync();
            return link.LinkId;
        }

        Task<bool> ILinkService.AcceptConnectionAsync(Guid linkId)
        {
            throw new NotImplementedException();
        }

        Task<bool> ILinkService.RejectConnectionAsync(Guid linkId)
        {
            throw new NotImplementedException();
        }
    }
}
