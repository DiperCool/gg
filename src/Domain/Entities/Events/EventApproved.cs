namespace CleanArchitecture.Domain.Entities.Events;

public class EventApproved
{
    public Guid Id { get; set; }
    public string EventId { get; set; } = String.Empty;
    public Event Event { get; set; } = null!;
    
    public Guid? UserId { get; set; }
    public User? User { get;set; }
    
    public Guid? TeamId { get; set; }
    public Team? Team { get; set; }
}