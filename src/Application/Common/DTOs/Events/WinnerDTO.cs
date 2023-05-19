using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.Events;

namespace CleanArchitecture.Application.Common.DTOs.Events;

public class WinnerDTO : IMapFrom<Winner>
{
    public Guid? Id { get; set; }
    public void Mapping(AutoMapper.Profile profile)
    {
        profile.CreateMap<Winner, WinnerDTO>()
            .ForMember(x => x.Id,
                opt => opt.MapFrom(item => item.TeamId ?? item.UserId));
    }
    
}