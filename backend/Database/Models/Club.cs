using System.ComponentModel.DataAnnotations.Schema;

namespace Database;

public class Club
{
    public int id { get; set; }

    public bool IsPublic { get; set; }
    
    public string Name { get; set; }
    
    public string Desciption { get; set; }

    public ICollection<ClubMember> ClubMembers { get; set; } = new List<ClubMember>();
    
}