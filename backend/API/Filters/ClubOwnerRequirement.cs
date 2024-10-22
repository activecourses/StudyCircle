using Microsoft.AspNetCore.Authorization;

namespace API.Filters
{
    public class ClubOwnerRequirement : IAuthorizationRequirement
    {
        public string GroupId { get; }

        public ClubOwnerRequirement(string groupId)
        {
            GroupId = groupId;
        }
        public override string ToString()
        {
            return $"Owner#{GroupId}";
        }
    }
}