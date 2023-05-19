using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Events.Command.CancelParticipation;
[Authorize]
public class CancelParticipationCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
    public string EventId { get; set; } = String.Empty;
}

public class CancelParticipationCommandHandler : IRequestHandler<CancelParticipationCommand, Unit>
{
    private IApplicationDbContext _context;
    private ICurrentUserService _current;

    public CancelParticipationCommandHandler(IApplicationDbContext context, ICurrentUserService current)
    {
        _context = context;
        _current = current;
    }

    public async Task<Unit> Handle(CancelParticipationCommand request, CancellationToken cancellationToken)
    {
        Event gameEvent = await _context.Events.FirstOrDefaultAsync(x => x.Id == request.EventId, cancellationToken: cancellationToken) ??
                          throw new BadRequestException("Not found event");
        Team? team = await _context.Teams.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
        User? user = await _context.Users.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
        if (team == null && user == null)
        {
            throw new BadRequestException("User or team not found");
        }

        if (team!=null && !await _context.TeamUsers.AnyAsync(x => x.TeamId == request.Id && x.UserId==_current.UserIdGuid, cancellationToken: cancellationToken))
        {
            throw new BadRequestException("You're not teammate of this team");
        }
        EventParticipant participant = await _context.EventParticipants.FirstOrDefaultAsync(x =>
                x.EventId == request.EventId && (x.TeamId == request.Id || x.UserId == request.Id),
            cancellationToken: cancellationToken) ?? throw new BadRequestException("You're not participant");
        participant.IsApproved = "False";
        _context.EventParticipants.Update(participant);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}