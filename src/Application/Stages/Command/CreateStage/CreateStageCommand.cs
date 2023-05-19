using CleanArchitecture.Domain.Entities.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Stages.Command.CreateStage;
[EmployeeAuthorize("Organizer")]
public class CreateStageCommand : IRequest<string>
{
    public string EventId { get; set; } = String.Empty;
    public StageWithWinnersModel Stage { get; set; } = null!;
}

public class CreateStageCommandHandler : IRequestHandler<CreateStageCommand, string>
{
    private IApplicationDbContext _context;
    private ICurrentUserService _current;

    public CreateStageCommandHandler(IApplicationDbContext context, ICurrentUserService current)
    {
        _context = context;
        _current = current;
    }

    public async Task<string> Handle(CreateStageCommand request, CancellationToken cancellationToken)
    {
        Event eventGame =
            await _context.Events
                .Include(x=>x.Stages)
                .FirstOrDefaultAsync(x =>
                x.Id == request.EventId && x.OrganizerId == _current.UserIdGuid, cancellationToken: cancellationToken) ??
            throw new BadRequestException("Event not found");
        int stageCount = await _context.Stages.Where(x => x.EventId == request.EventId).CountAsync(cancellationToken: cancellationToken);
        List<Winner> winners = new()
        {
        };
        foreach (WinnerModel winnerModel in request.Stage.Winners)
        {
            Winner winner = new() { };
            if (await _context.Users.AnyAsync(x => x.Id == winnerModel.Id, cancellationToken: cancellationToken))
            {
                winner.UserId = winnerModel.Id;
            }
            else if (await _context.Teams.AnyAsync(x => x.Id == winnerModel.Id, cancellationToken: cancellationToken))
            {
                winner.TeamId = winnerModel.Id;
            }
            else
            {
                throw new BadRequestException("User or team not found");
            }

            winners.Add(winner);

        }
        Stage stage = new Stage()
        {
            EventId = request.EventId,
            Id =  $"{eventGame.Id}-{stageCount + 1}-stage",
            Name = request.Stage.Name,
            StageStart = request.Stage.StageStart,
            Winners =winners,
            Groups = request.Stage.Groups.Select((group, i) => new Group()
            {
                Id = $"{eventGame.Id}-{stageCount + 1}-stage-{i+1}-group",
                Name = group.Name,
                ConfirmationTimeEnd = group.ConfirmationTimeEnd,
                ConfirmationTimeStart = group.ConfirmationTimeStart,
                GroupStart = group.GroupStart,
                ReserveConfirmationTimeEnd = group.ReserveConfirmationTimeEnd,
                Map = group.Map,
                PaidSlots = group.PaidSlots,
                GroupModerators =
                    group.GroupModerators.Select(moderator =>
                        new GroupModerator() { EmployeeId = moderator.EmployeeId }).ToList(),
                LobbyId = group.LobbyId,
                LobbyPassword = group.LobbyPassword,
                ReserveSlotsQuantity = group.ReserveSlotsQuantity,
                SlotPrice = group.SlotPrice,
                SlotsQuantity = group.SlotsQuantity
            }).ToList(),
            View = request.Stage.View
        };
        await _context.Stages.AddAsync(stage, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return stage.Id;
    }
    
}