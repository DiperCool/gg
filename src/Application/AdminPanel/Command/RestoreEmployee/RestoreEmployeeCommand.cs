using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.AdminPanel.Command.RestoreEmployee;

public class RestoreEmployeeCommand : IRequest<Unit>
{
    public Guid EmployeeId { get; set; }
}

public class RestoreEmployeeCommandHandler : IRequestHandler<RestoreEmployeeCommand, Unit>
{
    private IApplicationDbContext _context;
    private ICurrentUserService _current;
    private readonly Dictionary<string,List<string>> _dictionary;
    private IEmployeeLogService _log;
    public RestoreEmployeeCommandHandler(IApplicationDbContext context, ICurrentUserService current, IEmployeeLogService log)
    {
        _context = context;
        _current = current;
        _log = log;
        _dictionary = new Dictionary<string, List<string>>()
        {
            { "Admin", new List<string>(){ "Owner" } },
            { "Organizer", new List<string>(){ "Admin", "Owner" } },
            { "Moderator", new List<string>(){ "Admin", "Owner" } },
            { "EventModerator", new List<string>(){ "Admin", "Owner", "Organizer" } },
        };
    }

    public async Task<Unit> Handle(RestoreEmployeeCommand request, CancellationToken cancellationToken)
    {
        Employee employeeToDel =await _context.Employees.Include(x=>x.Role).FirstOrDefaultAsync(x => x.Id == request.EmployeeId, cancellationToken: cancellationToken) ??
                                throw new BadRequestException("Employee doesn't exists");
        Employee employeeDel = await _context.Employees.Include(x=>x.Role).FirstAsync(x => x.Id == _current.UserIdGuid, cancellationToken: cancellationToken);
        if (!_dictionary.ContainsKey(employeeToDel.Role.Role))
        {
            throw new BadRequestException();
        }

        if (!_dictionary[employeeToDel.Role.Role].Contains(employeeDel.Role.Role))
        {
            throw new ForbiddenAccessException();
        }

        employeeToDel.IsDeleted = false;
        employeeToDel.DeletedById = null;
        _context.Employees.Update(employeeToDel);
        await _context.SaveChangesAsync(cancellationToken);
        await _log.Log(_current.UserIdGuid, LogsEnum.RestoreEmployee, _current.UserIdGuid, employeeToDel.Id);
        return Unit.Value;
    }
}
