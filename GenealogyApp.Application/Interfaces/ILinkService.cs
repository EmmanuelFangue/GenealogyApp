namespace GenealogyApp.Application.Interfaces
{
    public interface ILinkService
    {
        Task<Guid> SendConnectionRequestAsync(Guid requesterId, Guid receiverId, string relationType);
        Task<bool> AcceptConnectionAsync(Guid linkId);
        Task<bool> RejectConnectionAsync(Guid linkId);
    }
}