using GenealogyApp.Application.DTOs;
using GenealogyApp.Application.Interfaces;
using GenealogyApp.Domain.Entities;
using GenealogyApp.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace GenealogyApp.Application.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly GenealogyDbContext _db;

        public PhotoService(GenealogyDbContext db)
        {
            _db = db;
        }

        // Stockage local, à remplacer éventuellement par Azure Blob, etc.
        private async Task<string> SaveFileAsync(IFormFile file)
        {
            var folder = Path.Combine("wwwroot", "uploads", Guid.NewGuid().ToString());
            Directory.CreateDirectory(folder);

            var filePath = Path.Combine(folder, file.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            // Retourne l'URL relative
            return filePath.Replace("\\", "/");
        }

        public async Task<PhotoDto?> AddPhotoAsync(PhotoUploadDto dto)
        {
            var member = await _db.FamilyMembers
                .Include(f => f.Photos)
                .FirstOrDefaultAsync(f => f.MemberId == dto.MemberId);

            if (member == null)
                return null;

            if (await _db.Photos.CountAsync(p => p.MemberId == dto.MemberId) >= 20)
                return null;

            var url = await SaveFileAsync(dto.File);

            var photo = new Photo
            {
                MemberId = dto.MemberId,
                Url = url,
                UploadedAt = DateTime.UtcNow
            };

            _db.Photos.Add(photo);
            await _db.SaveChangesAsync();

            return new PhotoDto
            {
                PhotoId = photo.PhotoId,
                Url = url,
                UploadedAt = photo.UploadedAt
            };
        }


        public async Task<IEnumerable<PhotoDto>> GetPhotosAsync(Guid memberId)
        {
            return await _db.Photos
                .Where(p => p.MemberId == memberId)
                .Select(p => new PhotoDto
                {
                    PhotoId = p.PhotoId,
                    Url = p.Url,
                    UploadedAt = p.UploadedAt
                })
                .ToListAsync();
        }

        public async Task<bool> DeletePhotoAsync(Guid memberId, Guid photoId)
        {
            var photo = await _db.Photos.FirstOrDefaultAsync(p => p.PhotoId == photoId && p.MemberId == memberId);
            if (photo == null)
                return false;

            _db.Photos.Remove(photo);
            await _db.SaveChangesAsync();

            // Optionnel : delete le fichier sur le disque

            return true;
        }

        public async Task<bool> SetProfilePhotoAsync(Guid memberId, Guid photoId)
        {
            var member = await _db.FamilyMembers.FindAsync(memberId);
            var photo = await _db.Photos.FirstOrDefaultAsync(p => p.PhotoId == photoId && p.MemberId == memberId);

            if (member == null || photo == null) return false;

            member.ProfilePhotoUrl = photo.Url;
            await _db.SaveChangesAsync();
            return true;
        }

    }
}