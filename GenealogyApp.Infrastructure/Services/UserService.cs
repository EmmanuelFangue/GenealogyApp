using GenealogyApp.Application.DTOs;
using GenealogyApp.Application.Interfaces;
using GenealogyApp.Domain.Entities;
using GenealogyApp.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;

namespace GenealogyApp.Infrastructure.Services
{

    public class UserService : IUserService
    {
        private readonly GenealogyDbContext _db;
        private readonly IConfiguration _config;
        private readonly IPasswordHasher<User> _hasher;

        public UserService(GenealogyDbContext db, IConfiguration config, IPasswordHasher<User> hasher)
        {
            _db = db;
            _config = config;
            _hasher = hasher;
        }

        public async Task<UserDto?> RegisterAsync(RegisterUserDto dto)
        {
            if (await _db.Users.AnyAsync(u => u.Username == dto.Username || u.Email == dto.Email))
                return null;

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                TwoFactorEnabled = false,
                CreatedAt = DateTime.UtcNow
            };
            user.PasswordHash = _hasher.HashPassword(user, dto.Password);

            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            return ToDto(user);
        }

        public async Task<LoginResponseDto?> LoginAsync(LoginDto dto)
        {
            var user = await _db.Users
                .FirstOrDefaultAsync(u => u.Username == dto.UsernameOrEmail || u.Email == dto.UsernameOrEmail);
            if (user is null) return null;

            var verify = _hasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (verify == PasswordVerificationResult.Failed) return null;

            var token = GenerateJwtToken(user.UserId, user.Username);
            return new LoginResponseDto { UserId = user.UserId, Token = token, TwoFactorEnabled = user.TwoFactorEnabled };
        }

        public string GenerateJwtToken(Guid userId, string username)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, username),
                // Ajoute des rôles si besoin
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(12),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<bool> Activate2FAAsync(Activate2FADto dto)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.UserId == dto.UserId);
            if (user == null) return false;

            // Vérifie le code (exemple: "123456", à remplacer par ta logique réelle)
            if (dto.Code == "123456")
            {
                user.TwoFactorEnabled = true;
                await _db.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<UserDto?> GetUserAsync(Guid userId)
        {
            var user = await _db.Users
                        .Include(u => u.FamilyMembers)
                        .FirstOrDefaultAsync(u => u.UserId == userId);

            return user != null ? ToDto(user) : null;
        }

        public async Task<bool> UpdateUserAsync(Guid userId, UpdateUserDto dto)
        {
            var user = await _db.Users.FindAsync(userId);
            if (user == null) return false;

            if (!string.IsNullOrEmpty(dto.Email)) user.Email = dto.Email;
            if (!string.IsNullOrEmpty(dto.PhoneNumber)) user.PhoneNumber = dto.PhoneNumber;
            if (!string.IsNullOrEmpty(dto.Password)) user.PasswordHash = HashPassword(dto.Password);

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUserAsync(Guid userId)
        {
            var user = await _db.Users.FindAsync(userId);
            if (user == null) return false;

            _db.Users.Remove(user);
            await _db.SaveChangesAsync();
            return true;
        }

        private string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        private bool VerifyPassword(string password, string hash)
        {
            return HashPassword(password) == hash;
        }

        private UserDto ToDto(User user)
        {
            var selfMember = user.FamilyMembers.FirstOrDefault(m => m.RelationToUser == "self");

            return new UserDto
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                TwoFactorEnabled = user.TwoFactorEnabled,
                ProfileMemberId = selfMember?.MemberId
            };
        }
    }


}