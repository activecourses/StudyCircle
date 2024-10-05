namespace Database;

public class Club
{
    public int id { get; set; }

    public bool IsPublic { get; set; }

    public required string Name { get; set; }

    public required string Desciption { get; set; }

    public ICollection<ClubMember> ClubMembers { get; set; } = new List<ClubMember>();
}