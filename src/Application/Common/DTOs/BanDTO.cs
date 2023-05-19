using CleanArchitecture.Domain.Entities;
using Profile = AutoMapper.Profile;

namespace CleanArchitecture.Application.Common.DTOs;

public class BanDTO : IMapFrom<Ban>, IMapFrom<ShadowBan>
{
    public DateTime? To { get; set; }
    public bool Active => To != null && To >= DateTime.UtcNow;

    public Guid? EmployeeId { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Ban, BanDTO>();
        profile.CreateMap<ShadowBan, BanDTO>();
    }
}