using AutoMapper;
using CleanArchitecture.Domain.Entities.Events;

namespace CleanArchitecture.Application.Common.DTOs.Events;

public class PlayerStatsDTO : IMapFrom<PlayerStats>
{
    public Guid? ParticipantId  { get; set; }
    public string ParticipantName { get; set; } = String.Empty;
    public MediaFileDTO? ParticipantPicture { get; set; }
    public int Place { get; set; }
    public int Points { get; set; }
    public int Kills { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<PlayerStats, PlayerStatsDTO>()
            .ForMember(x => x.ParticipantId,
                opt => opt.MapFrom(item => item.TeamId ?? item.UserId))
            .ForMember(x => x.ParticipantName,
                opt => opt.MapFrom(item => item.TeamId!=null? item.Team!.Title : item.User!.Profile.Nickname))
            .ForMember(x => x.ParticipantPicture,
                opt => opt.MapFrom(item => item.TeamId!=null? item.Team!.Logo : item.User!.Profile.ProfilePicture));
    }

}