using CleanArchitecture.Domain.Entities.Events;
using CleanArchitecture.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Stages.Command.UpdateStage;
[EmployeeAuthorize("Organizer")]
public class UpdateStageCommand : IRequest<Unit>
{
    public string Id { get; set; } = String.Empty;
    public string Name { get; set; } = String.Empty;
    public DateTime StageStart { get; set; }
    public List<WinnerModel> Winners { get; set; } = new();
    public View View { get; set; }
}

public class UpdateStageCommandHandler : IRequestHandler<UpdateStageCommand, Unit>
{
    private IApplicationDbContext _context;
    private ICurrentUserService _current;

    public UpdateStageCommandHandler(IApplicationDbContext context, ICurrentUserService current)
    {
        _context = context;
        _current = current;
    }

    public async Task<Unit> Handle(UpdateStageCommand request, CancellationToken cancellationToken)
    {
        Stage stage =
            await _context.Stages
                .Include(x=>x.Winners)
                .FirstOrDefaultAsync(x =>
                x.Id == request.Id && x.Event.OrganizerId == _current.UserIdGuid, cancellationToken: cancellationToken) ??
            throw new BadRequestException("Not found");
        List<Winner> winners = new()
        {
        };
        foreach (WinnerModel winnerModel in request.Winners)
        {
            Winner winner = new() { StageId = stage.Id};
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
        stage.Name = request.Name;
        stage.StageStart = request.StageStart;
        stage.Winners.Clear();
        stage.Winners = winners;
        stage.View = request.View;
        _context.Stages.Update(stage);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;

    }
}