using CleanArchitecture.Domain.Entities.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Groups.Command.CreateGroup;
[EmployeeAuthorize("Organizer")]

public class CreateGroupCommand: IRequest<Unit>
{
    public string StageId { get; set; } = String.Empty;
    public GroupModel GroupModel { get; set; } = null!;
}

public class CreateGroupCommandHandler : IRequestHandler<CreateGroupCommand, Unit>
{
    private IApplicationDbContext _context;
    private ICurrentUserService _current;

    public CreateGroupCommandHandler(IApplicationDbContext context, ICurrentUserService current)
    {
        _context = context;
        _current = current;
    }

    public async Task<Unit> Handle(CreateGroupCommand request, CancellationToken cancellationToken)
    {
        Stage stage =
            await _context.Stages.FirstOrDefaultAsync(x =>
                x.Id == request.StageId && x.Event.OrganizerId == _current.UserIdGuid, cancellationToken: cancellationToken) ??
            throw new BadRequestException("Not found");
        int groupCount = await _context.Groups.Where(x => x.StageId == request.StageId).CountAsync(cancellationToken: cancellationToken);
        foreach (Guid groupModerator in request.GroupModel.GroupModerators.Select(x=>x.EmployeeId))
        {
            if (!await _context.Employees.AnyAsync(x => x.Id == groupModerator, cancellationToken: cancellationToken))
            {
                throw new BadRequestException("Employee doesnt exists");
            }
        }
        Group group = new()
        {
            Id = stage.Id + "-" + (groupCount + 1) + "-group",
            StageId = stage.Id,
            Name = request.GroupModel.Name,
            ConfirmationTimeEnd = request.GroupModel.ConfirmationTimeEnd,
            ConfirmationTimeStart = request.GroupModel.ConfirmationTimeStart,
            GroupStart = request.GroupModel.GroupStart,
            Map = request.GroupModel.Map,
            PaidSlots = request.GroupModel.PaidSlots,
            ReserveConfirmationTimeEnd = request.GroupModel.ReserveConfirmationTimeEnd,
            GroupModerators =
                request.GroupModel.GroupModerators.Select(x => new GroupModerator() { EmployeeId = x.EmployeeId })
                    .ToList(),
            LobbyId = request.GroupModel.LobbyId,
            LobbyPassword = request.GroupModel.LobbyPassword,
            ReserveSlotsQuantity = request.GroupModel.ReserveSlotsQuantity,
            SlotPrice = request.GroupModel.SlotPrice,
            SlotsQuantity = request.GroupModel.SlotsQuantity
        };
        await _context.Groups.AddAsync(group, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}