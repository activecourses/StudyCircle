using Microsoft.AspNetCore.Authorization;

namespace API.Filters
{
    public class ClubMemberRequirement : IAuthorizationRequirement
    {
        public string GroupId { get; set; }

        public ClubMemberRequirement(string groupId)
        {
            GroupId = groupId;
        }
        public override string ToString()
        {
            return $"Member#{GroupId}";
        }
    }
}
