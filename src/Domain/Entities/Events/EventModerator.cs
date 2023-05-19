namespace CleanArchitecture.Domain.Entities.Events;

public class EventModerator
{
    public Guid Id { get; set; }
    
    public string EventId { get; set; } = String.Empty;
    public Event Event { get; set; } = null!;
    
    public Guid EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;
}