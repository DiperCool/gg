using AutoMapper;
using CleanArchitecture.Domain.Entities.Events;

namespace CleanArchitecture.Application.Common.DTOs.Events;

public class EventParticipantDTO: IMapFrom<EventParticipant>
{
    public string IsApproved { get; set; } = String.Empty;
    public Guid? Id { get; set; }
    public string? EventId { get; set; } = String.Empty;
    public void Mapping(Profile profile)
    {
        profile.CreateMap<EventParticipant, EventParticipantDTO>()
            .ForMember(x => x.Id,
                opt => opt.MapFrom(item => item.TeamId ?? item.UserId));
    }
}