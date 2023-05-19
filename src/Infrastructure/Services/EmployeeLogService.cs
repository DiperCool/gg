using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Logs;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Enums;

namespace CleanArchitecture.Infrastructure.Services;

public class EmployeeLogService : IEmployeeLogService
{
    private IApplicationDbContext _context;

    public EmployeeLogService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Log(Guid employeeId, LogsEnum logsEnum, params object[] args)
    {
        EmployeeLog employeeLog = new()
        {
            EmployeeId = employeeId, Log = String.Format(LogsDictionary.Get[logsEnum], args), LoggedAt = DateTime.UtcNow, LogType = logsEnum
        };
        await _context.EmployeeLogs.AddAsync(employeeLog);
        await _context.SaveChangesAsync(CancellationToken.None);
    }
}