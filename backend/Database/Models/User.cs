using System.ComponentModel.DataAnnotations.Schema;

namespace Database;

public class User
{
    public int id { get; set; }

    public string Username { get; set; }

    public string Password { get; set; }

    public string Email { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string? City { get; set; }

    public string? Country { get; set; }

    public ICollection<ClubMember> ClubMember { get; set; } = new List<ClubMember>();
}