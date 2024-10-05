namespace Database;

public class User
{
    public int Id { get; set; }

    public required string Username { get; set; }

    public required string Password { get; set; }

    public required string Email { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public string? City { get; set; }

    public string? Country { get; set; }

    public ICollection<ClubMember> ClubMember { get; set; } = new List<ClubMember>();
}