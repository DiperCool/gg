using CleanArchitecture.Application.Common.Logs;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.EmployeeProfiles;
using CleanArchitecture.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.AdminPanel.Command.CreateEmployee;
[EmployeeAuthorize]
public class CreateEmployeeCommand: IRequest<Guid>
{
    public string Nickname { get; set; } = String.Empty;
    public string Role { get; set; } = String.Empty;
    public string Password { get; set; } = String.Empty;
    public string ConfirmPassword { get; set; } = String.Empty; 
}

public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, Guid>
{
    private IApplicationDbContext _application;
    private ICurrentUserService _current;
    private Dictionary<string, List<string>> _allowedRoles;
    private IHashPassword _hashPassword;
    private IEmployeeLogService _log;
    public CreateEmployeeCommandHandler(IApplicationDbContext application, ICurrentUserService current, IHashPassword hashPassword, IEmployeeLogService log)
    {
        _application = application;
        _current = current;
        _hashPassword = hashPassword;
        _log = log;
        _allowedRoles = new Dictionary<string, List<string>>
        {
            { "Admin", new List<string>(){ "Owner" } },
            { "Organizer", new List<string>(){ "Admin", "Owner" } },
            { "Moderator", new List<string>(){ "Admin", "Owner" } },
            { "NewsEditor", new List<string>(){ "Admin", "Owner" } },
            { "EventModerator", new List<string>(){ "Admin", "Owner", "Organizer" } },
        };
    }

    public async Task<Guid> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        string role = await _application.Employees.Where(x => x.Id == _current.UserIdGuid).Select(x => x.Role.Role)
            .FirstAsync(cancellationToken: cancellationToken);
        if (!_allowedRoles.ContainsKey(request.Role))
        {
            throw new BadRequestException();
        }

        if (!_allowedRoles[request.Role].Contains(role))
        {
            throw new ForbiddenAccessException();
        }

        Employee employee = new()
        {
            RoleId = await _application.EmployeeRoles.Where(x => x.Role == request.Role).Select(x => x.Id)
                .FirstAsync(cancellationToken: cancellationToken),
            Nickname = request.Nickname,
            Profile = new EmployeeProfile(),
            Password = _hashPassword.Hash(request.Password),
            CreatedAt = DateTime.UtcNow,
            CreatedById = _current.UserIdGuid
        };
        if (request.Role == "Organizer")
        {
            employee.Profile = new OrganizerProfile();
        }

        if (request.Role == "NewsEditor")
        {
            employee.Profile = new NewsEditorProfile();
        }
        await _application.Employees.AddAsync(employee, cancellationToken);
        await _application.SaveChangesAsync(cancellationToken);
        await _log.Log(_current.UserIdGuid, LogsEnum.CreateEmployee, _current.UserIdGuid, employee.Id );
        return employee.Id;

    }
}