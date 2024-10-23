using Database.Models;
using Microsoft.AspNetCore.Authorization;

namespace Business.Authorization
{
    public class ClubAuthorizationRequirement : IAuthorizationRequirement
    {
        public string ClubId { get; }
        public ClubRole RequiredRole { get; }

        public ClubAuthorizationRequirement(string clubId, ClubRole requiredRole)
        {
            ClubId = clubId;
            RequiredRole = requiredRole;
        }
    }
}