namespace CleanArchitecture.Domain.Entities.Events;

public class GroupModerator
{
    public Guid Id { get; set; }
    public string GroupId { get; set; } = String.Empty;
    public Group Group { get; set; } = null!;
    
    public Guid EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;
}