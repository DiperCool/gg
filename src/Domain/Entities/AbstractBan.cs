namespace CleanArchitecture.Domain.Entities;

public class AbstractBan
{
    public Guid Id { get; set; }
    
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    
    public DateTime? To { get; set; }
    public bool Active => To != null && To >= DateTime.UtcNow;

    public Guid? EmployeeId { get; set; }
    public Employee? Employee { get; set; } = null!;
}