namespace Database;

public interface ITrackable
{
    public DateTime CreatedAt { get; set; }

    public DateTime DeletedAt { get; set; }


}