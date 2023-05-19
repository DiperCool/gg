namespace CleanArchitecture.Domain.Entities.Events;

public class PlayerStats
{
    public Guid Id { get; set; }
    public Guid? UserId { get; set; }
    public User? User { get; set; } = null!;
    
    public Guid? TeamId { get; set; }
    public Team? Team { get; set; } = null!;
    public string GroupId { get; set; } = String.Empty;
    public Group Group { get; set; } = null!;
    public int Place { get; set; }
    public int Points { get; set; }

    public int Kills { get; set; }

}