namespace CleanArchitecture.Domain.Entities;

public class TeamUser
{
    public Guid Id { get; set; }
    public Guid TeammateId { get; set; }
    public Teammate Teammate { get; set; } = null!;
    public Guid TeamId { get; set; }
    public User User { get; set; } = null!;
    public Guid UserId { get; set; }
    public Team Team { get; set; } = null!;
}