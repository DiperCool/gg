using System.Runtime.Intrinsics.X86;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.Events;
using CleanArchitecture.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Groups.Command.JoinGroup;

public class JoinGroupCommand : IRequest<Unit>
{
    public string GroupId { get; set; } = String.Empty;
    public string EventId { get; set; } = String.Empty;
    public Guid ParticipantId { get; set; }

    public int SlotId { get; set; }
}

public class JoinGroupCommandHandler : IRequestHandler<JoinGroupCommand, Unit>
{
    private IApplicationDbContext _context;
    private ICurrentUserService _currentUserService;

    public JoinGroupCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Unit> Handle(JoinGroupCommand request, CancellationToken cancellationToken)
    {
        Event gameEvent = await _context.Events.FirstOrDefaultAsync(x => x.Id == request.EventId, cancellationToken: cancellationToken) ??
                          throw new BadRequestException("Not found event");
        Group group = await _context.Groups.FirstOrDefaultAsync(x => x.Id == request.GroupId, cancellationToken: cancellationToken) ?? throw new BadRequestException("Group not found");
        if (!await _context.EventParticipants.AnyAsync(x =>
                x.EventId == request.EventId &&x.IsApproved=="True"&&
                (x.TeamId == request.ParticipantId || x.UserId == request.ParticipantId), cancellationToken: cancellationToken))
        {
            throw new BadRequestException("You're unregistered on this event");
        }
        Team? team = await _context.Teams.FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == request.ParticipantId, cancellationToken: cancellationToken);
        User? user = await _context.Users.FirstOrDefaultAsync(x => x.Id == request.ParticipantId, cancellationToken: cancellationToken);
        if (team == null && user == null)
        {
            throw new BadRequestException("User or team not found");
        }

        if (team == null && request.ParticipantId != _currentUserService.UserIdGuid)
        {
            throw new ForbiddenAccessException();
        }
        List<ParticipantsUser> usersParticipants = new();
        Participant participant =
            await _context.Participants.Where(x => x.SlotId == request.SlotId && x.GroupId == request.GroupId)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken) ?? throw new BadRequestException("Slot not found");
        if (participant.TeamId != null || participant.UserId != null)
        {
            throw new BadRequestException("This slot is occupied");
        }
        participant.Team = team;
        participant.User = user;
        if (user != null)
        {
            usersParticipants.Add(new ParticipantsUser()
            {
                User = user,
                Participant = participant
            });
        }
        else
        {
            List<Guid> userIds = await _context.TeamUsers.Where(x => x.TeamId == request.ParticipantId && x.Teammate.TeammateType!=TeammateType.Manager).Select(x => x.UserId)
                .ToListAsync(cancellationToken: cancellationToken);
            foreach (Guid userId in userIds)
            {
                usersParticipants.Add(new ParticipantsUser()
                {
                    UserId = userId,
                    Participant = participant
                });
            }
        }
        _context.Participants.Update(participant);

        //await _context.UsersParticipants.AddRangeAsync(usersParticipants, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
