namespace Database;

public class Club
{
    public int Id { get; set; }

    public bool IsPublic { get; set; }

    public string Name { get; set; } = null!;

    public string Desciption { get; set; } = null!;

    public virtual ICollection<ClubMember> ClubMembers { get; set; } = new List<ClubMember>();
}