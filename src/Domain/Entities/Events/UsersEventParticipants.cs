namespace CleanArchitecture.Domain.Entities.Events;

public class UsersEventParticipants
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    
    public Guid ParticipantId { get; set; }
    public EventParticipant Participant { get; set; } = null!;
}