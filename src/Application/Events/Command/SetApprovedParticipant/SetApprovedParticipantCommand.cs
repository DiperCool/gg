using CleanArchitecture.Domain.Entities.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Events.Command.SetApprovedParticipant;
[EmployeeAuthorize("Organizer")]
public class SetApprovedParticipantCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
    public string EventId { get; set; } = String.Empty;
    public string IsApproved { get; set; } = String.Empty;
}

public class SetApprovedParticipantCommandHandler : IRequestHandler<SetApprovedParticipantCommand, Unit>
{
    private IApplicationDbContext _context;
    private ICurrentUserService _current;

    public SetApprovedParticipantCommandHandler(IApplicationDbContext context, ICurrentUserService current)
    {
        _context = context;
        _current = current;
    }

    public async Task<Unit> Handle(SetApprovedParticipantCommand request, CancellationToken cancellationToken)
    {
        EventParticipant participant = await _context.EventParticipants
                                      .Where(x => (x.UserId == request.Id || x.TeamId == request.Id) &&
                                                  x.Event!.OrganizerId == _current.UserIdGuid && x.Event.Id==request.EventId)
                                      .FirstOrDefaultAsync(cancellationToken: cancellationToken) ??
                                  throw new BadRequestException("You're not organizer or participant doesn't exist");
        participant.IsApproved = request.IsApproved;
        _context.EventParticipants.Update(participant);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;

    }
}