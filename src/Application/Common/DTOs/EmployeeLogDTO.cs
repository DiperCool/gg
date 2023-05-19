using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Enums;

namespace CleanArchitecture.Application.Common.DTOs;

public class EmployeeLogDTO : IMapFrom<EmployeeLog>
{
    public LogsEnum LogType { get; set; }
    public string Log { get; set; } = String.Empty;
    
    public Guid EmployeeId { get; set; }
    
    public DateTime LoggedAt { get; set; }
}