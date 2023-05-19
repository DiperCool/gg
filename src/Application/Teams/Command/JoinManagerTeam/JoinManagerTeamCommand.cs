using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Teams.Command.JoinManagerTeam;

public class JoinManagerTeamCommand : IRequest<Unit>
{
    public string Code { get; set; }  = String.Empty;
}

public class JoinManagerTeamCommandHandler : IRequestHandler<JoinManagerTeamCommand, Unit>
{
    private IApplicationDbContext _context;
    private ICurrentUserService _current;

    public JoinManagerTeamCommandHandler(IApplicationDbContext context, ICurrentUserService current)
    {
        _context = context;
        _current = current;
    }

    public async Task<Unit> Handle(JoinManagerTeamCommand request, CancellationToken cancellationToken)
    {
        Team team = await _context.Teams.FirstOrDefaultAsync(x =>!x.IsDeleted && x.ManagerInvitationCodeLink == request.Code,
            cancellationToken: cancellationToken) ?? throw new BadRequestException("Code doesn't exist");
        if (await _context.TeamUsers.AnyAsync(x => x.TeamId == team.Id && x.Teammate.UserId == _current.UserIdGuid, cancellationToken: cancellationToken))
        {
            throw new BadRequestException();
        }
        TeamUser tu = new() { 
            Teammate = new Teammate()
            {
                TeammateType = TeammateType.Manager,
                UserId = _current.UserIdGuid
            },
            Team = team 
        };
        team.NumberOfMembers += 1;
        _context.Teams.Update(team);
        await _context.TeamUsers.AddAsync(tu, default);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}