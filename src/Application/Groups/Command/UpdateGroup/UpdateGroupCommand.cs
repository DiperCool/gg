using CleanArchitecture.Domain.Entities.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Groups.Command.UpdateGroup;
[EmployeeAuthorize("Organizer")]
public class UpdateGroupCommand : IRequest<Unit>
{
    public string GroupId { get; set; } = String.Empty;
    public GroupModelExtended Group { get; set; } = null!;
}

public class UpdateGroupCommandHandler : IRequestHandler<UpdateGroupCommand, Unit>
{
    private IApplicationDbContext _context;
    private ICurrentUserService _current;

    public UpdateGroupCommandHandler(IApplicationDbContext context, ICurrentUserService current)
    {
        _context = context;
        _current = current;
    }

    public async Task<Unit> Handle(UpdateGroupCommand request, CancellationToken cancellationToken)
    {
        Group group =
            await _context.Groups
                .Include(x=>x.GroupModerators)
                .Include(x=>x.Results)
                .FirstOrDefaultAsync(x =>
                x.Id == request.GroupId && x.Stage.Event.OrganizerId == _current.UserIdGuid, cancellationToken: cancellationToken) ??
            throw new BadRequestException("Not Found");
        foreach (Guid groupModerator in request.Group.GroupModerators.Select(x=>x.EmployeeId))
        {
            if (!await _context.Employees.AnyAsync(x => x.Id == groupModerator, cancellationToken: cancellationToken))
            {
                throw new BadRequestException("Employee doesn't exist");
            }
        }
        GroupModelExtended groupModel = request.Group;
        group.GroupModerators.Clear();
        group.GroupModerators = groupModel.GroupModerators
            .Select(x => new GroupModerator() { EmployeeId = x.EmployeeId }).ToList();
        group.Results.Clear();
        group.Results = groupModel.Results.Select(async statsModel => new PlayerStats()
        {
            TeamId = await _context.Teams.AnyAsync(x=>x.Id==statsModel.Id, cancellationToken: cancellationToken) ? statsModel.Id: null,
            UserId = await _context.Users.AnyAsync(x=>x.Id==statsModel.Id, cancellationToken: cancellationToken) ? statsModel.Id: null,
            Kills = statsModel.Kills,
            Points = statsModel.Points,
            Place = statsModel.Place
        }).Select(x=>x.Result).ToList();
        group.Name = groupModel.Name;
        group.PaidSlots = groupModel.PaidSlots;
        group.ReserveConfirmationTimeEnd = groupModel.ReserveConfirmationTimeEnd;
        group.ConfirmationTimeEnd = groupModel.ConfirmationTimeEnd;
        group.ConfirmationTimeStart = groupModel.ConfirmationTimeStart;
        group.GroupStart = groupModel.GroupStart;
        group.Map = groupModel.Map;
        group.LobbyId = groupModel.LobbyId;
        group.LobbyPassword = groupModel.LobbyPassword;
        group.ReserveSlotsQuantity = groupModel.ReserveSlotsQuantity;
        group.SlotPrice = groupModel.ReserveSlotsQuantity;
        group.SlotsQuantity = groupModel.SlotsQuantity;

        _context.Groups.Update(group);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}