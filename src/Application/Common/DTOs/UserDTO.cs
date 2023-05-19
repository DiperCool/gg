using CleanArchitecture.Application.Common.DTOs.Events;
using CleanArchitecture.Application.Common.DTOs.ShopItems;
using CleanArchitecture.Application.Common.DTOs.UserProfiles;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.Events;

namespace CleanArchitecture.Application.Common.DTOs;

public class UserDTO: IMapFrom<User>
{
    public Guid Id { get; set; }
    public bool IsDeleted { get; set; }
    public BanDTO Ban { get; set; } = null!;
    public BanDTO ShadowBan { get; set; } = null!;
    public int Coins { get; set; }
    public PrivateProfileDTO Profile { get; set; } = null!;
    public List<Guid> Teams { get; set; } = new();
    public List<DateTime> Logging = new();
    public UserStatisticDTO Statistic = null!;
    public List<Guid> ExTeams { get; set; } = new();
    public DateTime LastLogin { get; set; }
    public DateTime RegisteredAt { get; set; }
    public bool IsActive { get; set; }
    public void Mapping(AutoMapper.Profile profile)
    {
        profile.CreateMap<User, UserDTO>()
            .ForMember(x => x.Teams, opt => opt.MapFrom(x => x.TeamUsers.Where(x=>!x.Team.IsDeleted).Select(x => x.TeamId)))
            .ForMember(x => x.Logging,
                opt => opt.MapFrom(x => x.UserLogging.Where(x => !x.RefreshToken).Select(x => x.Time)))
            .ForMember(x => x.ExTeams, opt => opt.MapFrom(x => x.ExTeams.Select(x => x.TeamId)))
            .ForMember(x => x.RegisteredAt, opt => opt.MapFrom(x => x.RegisteredAt))
            .ForMember(x => x.LastLogin, opt => opt.MapFrom(x => x.ActiveUserLogging.UserLogging.Time))
            .ForMember(x => x.IsActive,
                opt => opt.MapFrom(x => x.ActiveUserLogging.UserLogging.Time.AddDays(14) >= DateTime.UtcNow));

    }
}