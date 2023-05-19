using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.Common.DTOs.UserProfiles;

public class PublicProfileDTO : IMapFrom<Profile>
{
    public string Nickname { get; set; } =String.Empty;
    public MediaFileDTO? ProfilePicture { get; set; }
}