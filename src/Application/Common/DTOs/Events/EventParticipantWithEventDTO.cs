using AutoMapper;
using CleanArchitecture.Domain.Entities.Events;

namespace CleanArchitecture.Application.Common.DTOs.Events;

public class EventParticipantWithEventDTO:IMapFrom<EventParticipant>
{
    public string IsApproved { get; set; } = String.Empty;
    public Guid Id { get; set; }
    public EventWithoutStageDTO Event { get; set; } = null!;

    public void Mapping(Profile profile)
    {
        profile.CreateMap<EventParticipant, EventParticipantWithEventDTO>()
            .ForMember(x => x.Id,
                opt => opt.MapFrom(item => item.TeamId ?? item.UserId));
    }
}