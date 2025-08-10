using GenealogyApp.Application.DTOs;
using Microsoft.AspNetCore.Http;

namespace GenealogyApp.Application.Interfaces
{
    public interface IPhotoService
    {
        Task<PhotoDto?> AddPhotoAsync(PhotoUploadDto dto);
        Task<IEnumerable<PhotoDto>> GetPhotosAsync(Guid memberId);
        Task<bool> DeletePhotoAsync(Guid memberId, Guid photoId);
        Task<bool> SetProfilePhotoAsync(Guid memberId, Guid photoId);
    }
}