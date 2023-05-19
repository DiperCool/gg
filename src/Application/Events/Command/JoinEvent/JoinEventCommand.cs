using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.Events;
using CleanArchitecture.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Events.Command.JoinEvent;
[Authorize]
public class JoinEventCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
    public string EventId { get; set; } = String.Empty;
    public string IsApproved { get; set; } = String.Empty;
}

public class JoinEventCommandHandler : IRequestHandler<JoinEventCommand, Unit>
{
    private IApplicationDbContext _context;
    private ICurrentUserService _currentUserService;

    public JoinEventCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Unit> Handle(JoinEventCommand request, CancellationToken cancellationToken)
    {
        Event gameEvent = await _context.Events.FirstOrDefaultAsync(x => x.Id == request.EventId, cancellationToken: cancellationToken) ??
                          throw new BadRequestException("Not found event");
        Team? team = await _context.Teams.FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == request.Id, cancellationToken: cancellationToken);
        User? user = await _context.Users.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
        if (team == null && user == null)
        {
            throw new BadRequestException("User or team not found");
        }

        if (team == null && request.Id != _currentUserService.UserIdGuid)
        {
            throw new ForbiddenAccessException();
        }
        List<UsersEventParticipants> usersParticipants = new();
        if (team!=null && !await _context.TeamUsers.AnyAsync(x => x.TeamId == request.Id && x.UserId==_currentUserService.UserIdGuid, cancellationToken: cancellationToken))
        {
            throw new BadRequestException("You're not teammate of this team");
        }
        if(await _context.UsersEventParticipants.AnyAsync(x=>x.UserId == _currentUserService.UserIdGuid&&x.Participant.EventId==request.EventId && x.Participant.TeamId==null, cancellationToken: cancellationToken))
        {
            throw new BadRequestException();
        }
        EventParticipant participant = new() { Team = team, User = user,IsApproved=request.IsApproved, EventId = request.EventId };

        if (user != null)
        {
            usersParticipants.Add(new UsersEventParticipants()
            {
                User = user,
                Participant = participant
            });
        }
        else
        {
            List<Guid> userIds = await _context.TeamUsers.Where(x => x.TeamId == request.Id && x.Teammate.TeammateType!=TeammateType.Manager).Select(x => x.UserId)
                .ToListAsync(cancellationToken: cancellationToken);
            foreach (Guid userId in userIds)
            {
                usersParticipants.Add(new UsersEventParticipants()
                {
                    UserId = userId,
                    Participant = participant
                });
            }
        }

        await _context.UsersEventParticipants.AddRangeAsync(usersParticipants, cancellationToken);
        await _context.EventParticipants.AddAsync(participant, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}