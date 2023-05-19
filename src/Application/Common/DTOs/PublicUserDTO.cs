using CleanArchitecture.Application.Common.DTOs.UserProfiles;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.Common.DTOs;

public class PublicUserDTO: IMapFrom<User>
{
    public Guid Id { get; set; }
    public bool IsDeleted { get; set; }
    public string Name { get; set; } = String.Empty;

    public UserStatisticDTO Statistic = null!;

    public PublicProfileDTO Profile { get; set; } = null!;
}