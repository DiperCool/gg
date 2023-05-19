using CleanArchitecture.Domain.Enums;

namespace CleanArchitecture.Domain.Entities;

public class EmployeeLog
{
    public Guid Id { get; set; }
    public LogsEnum LogType { get; set; }
    public string Log { get; set; } = String.Empty;
    
    public Guid EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;
    
    public DateTime LoggedAt { get; set; }
}