using CleanArchitecture.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Teams.Command.LeaveTeam;
[Authorize]
public class LeaveTeamCommand : IRequest<Unit>
{
    public Guid TeamId { get; set; }
}

public class LeaveTeamCommandHandler : IRequestHandler<LeaveTeamCommand, Unit>
{
    private IApplicationDbContext _context;
    private ICurrentUserService _currentUser;
    public LeaveTeamCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Unit> Handle(LeaveTeamCommand request, CancellationToken cancellationToken)
    {
        TeamUser tu =
            await _context.TeamUsers.Include(x=>x.Team).FirstOrDefaultAsync(x =>!x.Team.IsDeleted &&
                x.TeamId == request.TeamId && x.Teammate.UserId == _currentUser.UserIdGuid, cancellationToken: cancellationToken) ??
            throw new BadRequestException("You are not in this team");
        tu.Team.NumberOfMembers -= 1;
        _context.TeamUsers.Remove(tu);
        ExTeamUser exTeamUser = new() { UserId = _currentUser.UserIdGuid, TeamId = request.TeamId };
        await _context.ExTeamUsers.AddAsync(exTeamUser, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}