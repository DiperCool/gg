using AutoMapper;
using CleanArchitecture.Domain.Entities.Events;

namespace CleanArchitecture.Application.Common.DTOs.Events;

public class ParticipantDTO: IMapFrom<Participant>
{
    public Guid? ParticipantId { get; set; }
    public string ParticipantName { get; set; } = String.Empty;
    public int SlotId { get; set; }
    public MediaFileDTO? ParticipantPicture { get; set; }
    public bool ParticipationConfirmed { get; set; }
    public bool IsPaid { get; set; }
    public bool IsReserve { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Participant, ParticipantDTO>()
            .ForMember(x => x.ParticipantId,
                opt => opt.MapFrom(item => item.TeamId ?? item.UserId))
            .ForMember(x => x.ParticipantName,
                opt => opt.MapFrom(item => item.TeamId!=null? item.Team!.Title : item.User!.Profile.Nickname))
            .ForMember(x => x.ParticipantPicture,
                opt => opt.MapFrom(item => item.TeamId!=null? item.Team!.Logo : item.User!.Profile.ProfilePicture));
    }
}