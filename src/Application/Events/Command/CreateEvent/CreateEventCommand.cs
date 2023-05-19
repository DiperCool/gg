using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.EmployeeProfiles;
using CleanArchitecture.Domain.Entities.Events;
using CleanArchitecture.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Prize = CleanArchitecture.Domain.Entities.Events.Prize;

namespace CleanArchitecture.Application.Events.Command.CreateEvent;

[EmployeeAuthorize("Organizer")]
public class CreateEventCommand : IRequest<string>
{
    public EventType EventType { get; set; }
    public DateTime EventStart { get; set; }
    public DateTime EventEnd { get; set; }
    public string Picture { get; set; } = String.Empty;
    public bool IsSponsored { get ; set; }
    public string Title { get; set; } = String.Empty;
    public DateTime RegistrationStart { get; set; }

    public DateTime RegistrationEnd{ get; set; }
    public int TopWinners { get; set; }

    public Regime Regime { get; set; }
    public View View { get; set; }
    public int EntryPrice { get; set; }
    public bool IsPaid { get ; set; }
    public bool IsQuantityLimited { get; set; }
    public string Requirements { get; set; } = String.Empty;
    public string Description { get; set; } = String.Empty;

    public int MaxQuantity { get; set; }
    public PrizeModel Prize { get; set; } = null!;
    public List<StageModel> Stages { get; set; } = new();
}

public class CreateEventCommandHandler : IRequestHandler<CreateEventCommand, string>
{
    private IApplicationDbContext _context;
    private ICurrentUserService _current;
    private IEmployeeLogService _log;
    private IGeneratorId _generator;
    public CreateEventCommandHandler(IApplicationDbContext context, ICurrentUserService current, IEmployeeLogService log, IGeneratorId generator)
    {
        _context = context;
        _current = current;
        _log = log;
        _generator = generator;
    }

    public async Task<string> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        OrganizerProfile op =await _context.OrganizerProfiles.FirstAsync(x => x.EmployeeId == _current.UserIdGuid, cancellationToken: cancellationToken);
        int id = _generator.Generate();
        MediaFile? picture =
            await _context.MediaFiles.Where(x => x.ShortPath == "api/" + request.Picture)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        foreach (List<GroupModel> groupModels in request.Stages.Select(x=>x.Groups))
        {
            foreach (var group in groupModels)
            {
                if (!await _context.Employees.AnyAsync(x =>group.GroupModerators.Select(x=>x.EmployeeId).Contains(x.Id), cancellationToken: cancellationToken))
                {
                    throw new BadRequestException("Employee doesnt exists");
                }
            }
        }
        Event eventGame = new()
        {
            Id= id+"-event",
            Title = request.Title,
            Description = request.Description,
            IsApproved= false,
            TopWinners = request.TopWinners,
            Picture = picture,
            EventType = request.EventType,
            EventStart = request.EventStart,
            RegistrationStart = request.RegistrationStart,
            RegistrationEnd = request.RegistrationEnd,
            Regime = request.Regime,
            View = request.View,
            EventEnd = request.EventEnd,
            EntryPrice = request.EntryPrice,
            OrganizerId =_current.UserIdGuid,
            IsPaid = request.IsPaid,
            IsSponsored = request.IsSponsored,
            IsQuantityLimited = request.IsQuantityLimited,
            Requirements = request.Requirements,
            MaxQuantity = request.MaxQuantity,
            Prize = new Prize()
            {
               Pool = request.Prize.Pool,
               PrizePerKill = request.Prize.PrizePerKill,
               PlacementPrizes = request.Prize.PlacementPrizes.Select(x=>new PlacementPrize()
               {
                   Number = x.Number,
                   Prize = x.Prize
               }).ToList()
            }
        };
        eventGame.Stages = request.Stages.Select((stage, stageCount) => new Stage()
        {
            Event = eventGame,
            Id = $"{eventGame.Id}-{stageCount+1}-stage",
            Name = stage.Name,
            StageStart = stage.StageStart,
            View = stage.View,
            Groups = stage.Groups.Select((group, groupCount) => new Group()
            {
                Id = $"{eventGame.Id}-{stageCount+1}-stage-{groupCount+1}-group",
                Name = group.Name,
                ConfirmationTimeEnd = group.ConfirmationTimeEnd,
                ConfirmationTimeStart = group.ConfirmationTimeStart,
                GroupStart = group.GroupStart,
                Map = group.Map,
                PaidSlots = group.PaidSlots,
                GroupModerators = group.GroupModerators.Select(moderator=>new GroupModerator()
                {
                    EmployeeId = moderator.EmployeeId
                }).ToList(),
                Participants = group.Participants.Select(x=> new Participant()
                {
                    SlotId = x.SlotId,
                    ParticipationConfirmed = x.ParticipationConfirmed,
                    IsPaid = false,
                    IsReserve = false,
                    EventId = eventGame.Id
                }).Concat(group.PaidParticipants.Select(x=>new Participant()
                {
                    SlotId = x.SlotId,
                    ParticipationConfirmed = x.ParticipationConfirmed,
                    IsPaid = true,
                    IsReserve = false,
                    EventId = eventGame.Id
                })).Concat(group.ReserveParticipants.Select(x=>new Participant()
                {
                    SlotId = x.SlotId,
                    ParticipationConfirmed = x.ParticipationConfirmed,
                    IsPaid = false,
                    IsReserve = true,
                    EventId = eventGame.Id
                })).ToList(),
                ReserveConfirmationTimeEnd = group.ReserveConfirmationTimeEnd,
                LobbyId = group.LobbyId,
                LobbyPassword = group.LobbyPassword,
                ReserveSlotsQuantity = group.ReserveSlotsQuantity,
                SlotPrice = group.SlotPrice,
                SlotsQuantity = group.SlotsQuantity
            }).ToList()
        }).ToList();
        op.CreatedEvents.Add(eventGame);
        await _context.Events.AddAsync(eventGame, cancellationToken);
        _context.OrganizerProfiles.Update(op);
        await _context.SaveChangesAsync(cancellationToken);
        await _log.Log(_current.UserIdGuid, LogsEnum.CreateEvent, _current.UserIdGuid, eventGame.Id);
        return eventGame.Id;
    }
}