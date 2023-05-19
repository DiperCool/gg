using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.EmployeeProfiles;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Organizers.Commands.CreateOrganizer;
[EmployeeAuthorize("Admin")]
public class CreateOrganizerCommand: IRequest<Unit>
{
    public string Nickname { get; set; } = String.Empty;
    public string Name { get; set; } = String.Empty;
    public string Password { get; set; } = String.Empty;
    public string ConfirmPassword { get; set; } = String.Empty; 
}

public class CreateOrganizerCommandHandler : IRequestHandler<CreateOrganizerCommand, Unit>
{
    private IApplicationDbContext _application;

    private IHashPassword _hashPassword;

    public CreateOrganizerCommandHandler(IApplicationDbContext application, IHashPassword hashPassword)
    {
        _application = application;
        _hashPassword = hashPassword;
    }

    public async Task<Unit> Handle(CreateOrganizerCommand request, CancellationToken cancellationToken)
    {
        Employee employee = new()
        {
            RoleId = await _application.EmployeeRoles.Where(x => x.Role == "Organizer").Select(x => x.Id)
                .FirstAsync(cancellationToken: cancellationToken),
            Nickname = request.Nickname,
            Profile = new OrganizerProfile()
            {
                Name=request.Name
            },
            Password = _hashPassword.Hash(request.Password)
        };
        await _application.Employees.AddAsync(employee, cancellationToken);
        await _application.SaveChangesAsync(cancellationToken);
        return Unit.Value;

    }
}