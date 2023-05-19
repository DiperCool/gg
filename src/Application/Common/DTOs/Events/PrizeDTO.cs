using AutoMapper;
using CleanArchitecture.Domain.Entities.Events;

namespace CleanArchitecture.Application.Common.DTOs.Events;

public class PrizeDTO : IMapFrom<Prize>
{
    public int Pool { get; set; }
    public int PrizePerKill { get; set; }
    public List<PlacementPrizeDTO> PlacementPrizes { get; set; } = null!;
    public virtual void Mapping(Profile profile)
    {
        profile.CreateMap<Prize, PrizeDTO>()
            .ForMember(x => x.PlacementPrizes,
                opt => opt.MapFrom(item => item.PlacementPrizes.OrderBy(x => x.Number)));
    }
}