namespace CleanArchitecture.Domain.Entities;

public class UserLogging
{
    public Guid Id { get; set; }
    public DateTime Time { get; set; }
    public Guid? UserId { get; set; }
    public User? User { get; set; }
    public Guid? EmployeeId { get; set; }
    public Employee? Employee { get; set; }
    public bool RefreshToken { get; set; }
}