using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Teams.Command.RemoveTeam;
[Authorize]
public class RemoveTeamCommand : IRequest<Unit>
{
    public Guid TeamId { get; set; }
}

public class RemoveTeamCommandHandler : IRequestHandler<RemoveTeamCommand, Unit>
{
    private IApplicationDbContext _context;
    private ICurrentUserService _current;

    public RemoveTeamCommandHandler(IApplicationDbContext context, ICurrentUserService current)
    {
        _context = context;
        _current = current;
    }

    public async Task<Unit> Handle(RemoveTeamCommand request, CancellationToken cancellationToken)
    {
        Team team = await _context.Teams.FirstOrDefaultAsync(x =>
                        x.Id == request.TeamId && x.CreatorId == _current.UserIdGuid && !x.IsDeleted, cancellationToken: cancellationToken) ??
                    throw new BadRequestException("You're not creator");
        team.IsDeleted = true;
        List<UsersEventParticipants> usersParticipantsList =
            await _context.UsersEventParticipants.Where(x => x.Participant.TeamId == request.TeamId).ToListAsync(cancellationToken: cancellationToken);
        List<EventParticipant> eventParticipants =
            await _context.EventParticipants.Where(x => x.TeamId == request.TeamId).ToListAsync(cancellationToken: cancellationToken);
        _context.UsersEventParticipants.RemoveRange(usersParticipantsList);
        _context.EventParticipants.RemoveRange(eventParticipants);
        

        _context.Teams.Update(team);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}

