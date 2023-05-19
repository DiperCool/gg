using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.Common.DTOs.UserProfiles;

public class PrivateProfileDTO: IMapFrom<Profile>
{
    public string Name { get; set; } =String.Empty;
    public string Login { get; set; } =String.Empty;

    public string Email { get; set; }= String.Empty;
    
    public string Telegram { get; set; } =String.Empty;
    public string Youtube { get; set; } =String.Empty;
    public string Discord { get; set; } =String.Empty;
    public string PubgId { get; set; } = String.Empty;
    public string Nickname { get; set; } =String.Empty;
    public MediaFileDTO? ProfilePicture { get; set; } = null!;

}