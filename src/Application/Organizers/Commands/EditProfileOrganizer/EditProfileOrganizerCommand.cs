using CleanArchitecture.Domain.Entities.EmployeeProfiles;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Organizers.Commands.EditProfileOrganizer;
[EmployeeAuthorize("Organizer")]
public class EditProfileOrganizerCommand : IRequest<Unit>
{
    public string Name { get; set; } = String.Empty;
}

public class EditProfileOrganizerCommandHandler : IRequestHandler<EditProfileOrganizerCommand, Unit>
{
    private IApplicationDbContext _context;
    private ICurrentUserService _currentUser;

    public EditProfileOrganizerCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Unit> Handle(EditProfileOrganizerCommand request, CancellationToken cancellationToken)
    {
        OrganizerProfile profile =
            await _context.OrganizerProfiles.FirstAsync(x => x.EmployeeId == _currentUser.UserIdGuid, cancellationToken: cancellationToken);
        profile.Name = request.Name;
        _context.OrganizerProfiles.Update(profile);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}