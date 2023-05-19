namespace CleanArchitecture.Domain.Entities;

public class EmployeeRole
{
    public Guid Id { get; set; }
    public string Role { get; set; } =String.Empty;
    public List<Employee> Employees { get; set; } = new();
}