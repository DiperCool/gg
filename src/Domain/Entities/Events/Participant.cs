namespace CleanArchitecture.Domain.Entities.Events;

public class Participant
{
    public Guid Id { get; set; }
    public string GroupId { get; set; } = String.Empty;
    public Group Group { get; set; } = null!;
    
    public string EventId { get; set; } = String.Empty;
    public Event Event { get; set; } = null!;
    
    public Guid? UserId { get; set; }
    public User? User { get; set; }
    
    public Guid? TeamId { get; set; }
    public Team? Team { get; set; }
    
    public bool ParticipationConfirmed { get; set; }
    public int SlotId { get; set; }
    public bool IsPaid { get; set; }
    public bool IsReserve { get; set; }
    public List<ParticipantsUser> ParticipantsUsers { get; set; } = new();
}