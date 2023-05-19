using CleanArchitecture.Domain.Entities.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Events.Command.ApproveEvent;
[EmployeeAuthorize("Owner")]
public class ApproveEventCommand : IRequest<Unit>
{
    public string EventId { get; set; } = String.Empty;
}

public class ApproveEventCommandHandler : IRequestHandler<ApproveEventCommand, Unit>
{
    private IApplicationDbContext _context;

    public ApproveEventCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(ApproveEventCommand request, CancellationToken cancellationToken)
    {
        Event gameEvent = await _context.Events.FirstOrDefaultAsync(x => x.Id == request.EventId, cancellationToken: cancellationToken) ??
                          throw new BadRequestException("Not Found");
        gameEvent.IsApproved = true;
        _context.Events.Update(gameEvent);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}