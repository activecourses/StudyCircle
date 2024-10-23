namespace Database.Models
{
    public class ClubRoleAssignment
    {
        public int UserId { get; set; }
        public int ClubId { get; set; }
        public ClubRole Role { get; set; }
    }
}