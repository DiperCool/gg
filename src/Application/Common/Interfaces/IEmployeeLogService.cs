using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Enums;

namespace CleanArchitecture.Application.Common.Interfaces;

public interface IEmployeeLogService
{
    Task Log(Guid employeeId,  LogsEnum logsEnum,params object[] args);
}