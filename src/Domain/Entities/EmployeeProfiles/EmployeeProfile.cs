namespace CleanArchitecture.Domain.Entities.EmployeeProfiles;

public class EmployeeProfile
{
    public Guid Id { get; set; }
    public string Name { get; set; } = String.Empty;
    public Guid EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;
}