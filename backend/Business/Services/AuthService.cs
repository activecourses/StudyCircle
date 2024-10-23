using System.Security.Cryptography;
using Business.DTOs;
using Database.Models;
using Database;
using Microsoft.EntityFrameworkCore;

namespace Business.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IJwtService _jwtService;

        public AuthService(AppDbContext context, IJwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            if (await UserExistsAsync(registerDto.Username))
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Username already exists"
                };
            }

            if (await EmailExistsAsync(registerDto.Email))
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Email already exists"
                };
            }

            var user = new User
            {
                Username = registerDto.Username,
                Password = HashPassword(registerDto.Password),
                Email = registerDto.Email,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                City = registerDto.City,
                Country = registerDto.Country
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var roles = new List<ClubRoleAssignment>();
            var token = _jwtService.GenerateToken(user, roles);

            return new AuthResponseDto
            {
                Success = true,
                Token = token,
                User = MapToUserDto(user)
            };
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == loginDto.Username);

            if (user == null || !VerifyPassword(loginDto.Password, user.Password))
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Invalid username or password"
                };
            }

            var roles = await _context.ClubMembers
                .Where(cm => cm.UserId == user.Id)
                .Select(cm => new ClubRoleAssignment
                {
                    UserId = cm.UserId,
                    ClubId = cm.ClubId,
                    Role = GetRoleFromMember(cm)
                })
                .ToListAsync();

            var token = _jwtService.GenerateToken(user, roles);

            return new AuthResponseDto
            {
                Success = true,
                Token = token,
                User = MapToUserDto(user)
            };
        }

        public async Task<bool> UserExistsAsync(string username)
        {
            return await _context.Users
                .AnyAsync(u => u.Username == username);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Users
                .AnyAsync(u => u.Email == email);
        }

        private static string HashPassword(string password)
        {
            using var hmac = new HMACSHA512();
            var salt = hmac.Key;
            var hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

            byte[] hashBytes = new byte[salt.Length + hash.Length];
            Array.Copy(salt, 0, hashBytes, 0, salt.Length);
            Array.Copy(hash, 0, hashBytes, salt.Length, hash.Length);

            return Convert.ToBase64String(hashBytes);
        }

        private static bool VerifyPassword(string password, string storedHash)
        {
            byte[] hashBytes = Convert.FromBase64String(storedHash);

            byte[] salt = new byte[128];
            Array.Copy(hashBytes, 0, salt, 0, salt.Length);

            using var hmac = new HMACSHA512(salt);
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != hashBytes[salt.Length + i])
                    return false;
            }

            return true;
        }

        private static ClubRole GetRoleFromMember(ClubMember member)
        {
            if (member.IsOwner) return ClubRole.Owner;
            if (member.IsModerator) return ClubRole.Moderator;
            return ClubRole.Member;
        }

        private static UserDto MapToUserDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                City = user.City,
                Country = user.Country
            };
        }
    }
}
