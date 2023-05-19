namespace CleanArchitecture.Domain.Entities;

public class ActiveUserLogging
{
    public Guid Id { get; set; }
    
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public Guid UserLoggingId { get; set; }
    public UserLogging UserLogging { get; set; } = null!;
}