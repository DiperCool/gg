using CleanArchitecture.Application.Common.DTOs;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.Events;
using CleanArchitecture.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Events.Command.UpdateEvent;
[EmployeeAuthorize("Organizer")]
public class UpdateEventCommand : IRequest<Unit>
{
    public string Id { get; set; } =String.Empty;
    public string Title { get; set; } = String.Empty;
    public EventType EventType { get; set; }
    public DateTime EventStart { get; set; }
    public DateTime EventEnd { get; set; }
    public int TopWinners { get; set; }
    public DateTime RegistrationStart { get; set; }
    public bool IsSponsored { get ; set; }

    public DateTime RegistrationEnd{ get; set; }
    public Regime Regime { get; set; }
    public View View { get; set; }
    public int EntryPrice { get; set; }
    public bool IsPaid { get ; set; }
    public bool IsQuantityLimited { get; set; }
    public string Requirements { get; set; } = String.Empty;

    public string Description { get; set; } = String.Empty;

    public int MaxQuantity { get; set; }
    public PrizeModel Prize { get; set; } = null!;

}

public class UpdateEventCommandHandler : IRequestHandler<UpdateEventCommand, Unit>
{
    private IApplicationDbContext _context;
    private ICurrentUserService _current;
    private IEmployeeLogService _log;

    public UpdateEventCommandHandler(IApplicationDbContext context, ICurrentUserService current, IEmployeeLogService log)
    {
        _context = context;
        _current = current;
        _log = log;
    }

    public async Task<Unit> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
    {
        Event gameEvent = await _context.Events
                              .Include(x=>x.Prize)
                              .ThenInclude(x=>x.PlacementPrizes)
                              .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken) ??
                          throw new BadRequestException("Event doesn't exist");
        gameEvent.EventType = request.EventType;
        gameEvent.EventStart = request.EventStart;
        gameEvent.Title = request.Title;
        gameEvent.TopWinners = request.TopWinners;
        gameEvent.RegistrationStart = request.RegistrationStart;
        gameEvent.RegistrationEnd = request.RegistrationEnd;
        gameEvent.Regime = request.Regime;
        gameEvent.View = request.View;
        gameEvent.EventEnd = request.EventEnd;
        gameEvent.EntryPrice = request.EntryPrice;
        gameEvent.IsSponsored = request.IsSponsored;
        gameEvent.IsPaid = request.IsPaid;
        gameEvent.IsQuantityLimited = request.IsQuantityLimited;
        gameEvent.Requirements = request.Requirements;
        gameEvent.Description = request.Description;
        gameEvent.MaxQuantity = request.MaxQuantity;
        gameEvent.Prize.PlacementPrizes.Clear();
        gameEvent.Prize.Pool = request.Prize.Pool;
        gameEvent.Prize.PrizePerKill = request.Prize.PrizePerKill;
        gameEvent.Prize.PlacementPrizes = request.Prize.PlacementPrizes.Select(x => new PlacementPrize()
        {
            Number = x.Number,
            Prize = x.Prize
        }).ToList();
        _context.Events.Update(gameEvent);
        await _context.SaveChangesAsync(cancellationToken);
        await _log.Log(_current.UserIdGuid, LogsEnum.EditEvent, _current.UserIdGuid, gameEvent.Id);

        return Unit.Value;
    }
}