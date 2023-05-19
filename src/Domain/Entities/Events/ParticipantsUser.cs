namespace CleanArchitecture.Domain.Entities.Events;

public class ParticipantsUser
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    
    public Guid ParticipantId { get; set; }
    public Participant Participant { get; set; } = null!;
}