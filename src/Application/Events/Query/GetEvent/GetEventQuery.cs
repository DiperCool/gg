using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArchitecture.Application.Common.DTOs;
using CleanArchitecture.Application.Common.DTOs.Employee;
using CleanArchitecture.Application.Common.DTOs.Events;
using CleanArchitecture.Application.Common.DTOs.PublicEmployeesProfiles;
using CleanArchitecture.Application.Common.DTOs.UserProfiles;
using CleanArchitecture.Domain.Entities.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Events.Query.GetEvent;

public class GetEventQuery: IRequest<EventDTO>
{
    public string Id { get; set; } =String.Empty;
}

public class GetEventQueryHandler : IRequestHandler<GetEventQuery, EventDTO>
{
    private IApplicationDbContext _context;
    private IMapper _mapper;

    public GetEventQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public string IsApproved { get; set; } = String.Empty;
    public Guid Id { get; set; }
    public string EventId { get; set; } = String.Empty;
    public void Mapping(Profile profile)
    {
        profile.CreateMap<EventParticipant, EventParticipantDTO>()
            .ForMember(x => x.Id,
                opt => opt.MapFrom(item => item.TeamId ?? item.UserId));
    }
    public async Task<EventDTO> Handle(GetEventQuery request, CancellationToken cancellationToken)
    {
        EventDTO eventGame = await _context.Events
            .Where(x=>x.Id==request.Id)
            .Select(e=>new EventDTO()
            {
                Id = e.Id,
                CurrentQuantity = e.CurrentQuantity,
                Description = e.Description,
                Title = e.Title,
                EntryPrice = e.EntryPrice,
                EventEnd = e.EventEnd,
                EventStart = e.EventStart,
                EventType = e.EventType,
                IsApproved = e.IsApproved,
                IsPaid = e.IsPaid,
                IsQuantityLimited = e.IsQuantityLimited,
                TopWinners = e.TopWinners,
                RegistrationStart = e.RegistrationStart,
                RegistrationEnd = e.RegistrationEnd,
                Regime = e.Regime,
                View = e.View,
                Participants = e.Participants.Select(pr=> new EventParticipantDTO()
                {
                    IsApproved = pr.IsApproved,
                    EventId = pr.EventId,
                    Id = pr.TeamId ?? pr.UserId
                }).ToList(),
                Prize = new PrizeDTO()
                {
                    PrizePerKill = e.Prize.PrizePerKill,
                    Pool = e.Prize.PrizePerKill,
                    PlacementPrizes = e.Prize.PlacementPrizes.Select(pp=> new PlacementPrizeDTO()
                    {
                       Prize = pp.Prize,
                       Number = pp.Number,
                    }).ToList(),
                },
                Organizer = new PublicEmployeeDTO()
                {
                    Id = e.Organizer.Id,
                    IsDeleted = e.Organizer.IsDeleted,
                    Nickname = e.Organizer.Nickname,
                    Profile = new PublicEmployeeProfileDTO()
                    {
                        Name = e.Organizer.Profile.Name
                    },
                    Role = e.Organizer.Role.Role
                },
                Requirements = e.Requirements,
                MaxQuantity = e.MaxQuantity,
                Picture = e.Picture != null? new MediaFileDTO()
                {
                    Path = e.Picture.ShortPath.Replace("api/","")
                }: null,
                Winner = e.Winner!=null? new WinnerDTO()
                {
                    Id = e.Winner.TeamId ?? e.Winner.UserId
                }:null,
                Stages = e.Stages.Select(st=>new StageDTO()
                {
                    Id = st.Id,
                    EventId = st.EventId,
                    Name = st.Name,
                    StageStart = st.StageStart,
                    Winners = st.Winners.Select(win=> new WinnerDTO()
                    {
                        Id = win.TeamId ?? win.UserId
                    }).ToList(),
                    View = st.View,
                    Groups = st.Groups.Select(gr=>new GroupDTO()
                    {
                        Id = gr.Id,
                        Name = gr.Name,
                        ConfirmationTimeEnd = gr.ConfirmationTimeEnd,
                        ConfirmationTimeStart = gr.ConfirmationTimeStart,
                        GroupStart = gr.GroupStart,
                        Map = gr.Map,
                        Results = gr.Results.Select(res=> new PlayerStatsDTO()
                        {
                            ParticipantId = res.TeamId ?? res.UserId,
                            ParticipantName = res.TeamId == null? res.User!.Profile.Nickname: res.Team!.Title,
                            ParticipantPicture = new MediaFileDTO()
                            {
                                Path = (res.TeamId==null ? res!.User!.Profile!.ProfilePicture!.ShortPath.Replace("api/",""): res.Team!.Logo.ShortPath.Replace("api/","")),
                            },
                            Place = res.Place,
                            Points = res.Points,
                            Kills = res.Kills
                        }).ToList(),
                        GroupModerators = gr.GroupModerators.Select(gm=> new GroupModeratorDTO()
                        {
                           EmployeeId = gm.EmployeeId
                        }).ToList(),
                        ReserveConfirmationTimeEnd = gr.ReserveConfirmationTimeEnd,
                        PaidSlots = gr.PaidSlots,
                        SlotPrice = gr.SlotPrice,
                        SlotsQuantity = gr.SlotsQuantity,
                        AllParticipants = gr.Participants.Select(pr=>new ParticipantDTO()
                        {
                            ParticipantId = pr.TeamId ?? pr.UserId,
                            SlotId = pr.SlotId,
                            IsPaid = pr.IsPaid,
                            IsReserve = pr.IsReserve,
                            ParticipantName = pr.TeamId == null? pr.User!.Profile.Nickname: pr.Team!.Title,
                            ParticipantPicture = new MediaFileDTO()
                            {
                                Path = (pr.TeamId==null ? pr!.User!.Profile!.ProfilePicture!.ShortPath.Replace("api/",""): pr.Team!.Logo.ShortPath.Replace("api/","")),
                            },
                            ParticipationConfirmed = pr.ParticipationConfirmed
                        }).ToList(),
                    }).ToList()
                }).ToList()

            })
            .FirstOrDefaultAsync(cancellationToken: cancellationToken) ?? throw new BadRequestException("Not found");
        foreach (var group in eventGame.Stages.SelectMany(stage => stage.Groups))
        {
            group.Participants =
                group.AllParticipants.Where(x => !x.IsReserve && !x.IsPaid).OrderBy(x => x.SlotId).ToList();
            group.PaidParticipants =
                group.AllParticipants.Where(x => !x.IsReserve && x.IsPaid).OrderBy(x => x.SlotId).ToList();
            group.ReserveParticipants =
                group.AllParticipants.Where(x => x.IsReserve && !x.IsPaid).OrderBy(x => x.SlotId).ToList();
        }
        return eventGame;
    }
}