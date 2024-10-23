using Database.Models;
using Database;
using System.Security.Claims;

namespace Business.Services
{
    public interface IJwtService
    {
        string GenerateToken(User user, IEnumerable<ClubRoleAssignment> roles);
    }
}