using Microsoft.AspNetCore.Authorization;

namespace API.Filters
{
    public class ClubModeratorRequirement : IAuthorizationRequirement
    {
        public string GroupId { get; }

        public ClubModeratorRequirement(string groupId)
        {
            GroupId = groupId;
        }
        public override string ToString()
        {
            return $"Moderator#{GroupId}";
        }
    }
}
