namespace CleanArchitecture.Domain.Entities;

public class Profile
{
    public Guid Id { get; set; }
    public string Name { get; set; } =String.Empty;
    public string Login { get; set; } =String.Empty;

    public string Email { get; set; }= String.Empty;
    
    public string Telegram { get; set; } =String.Empty;
    public string Youtube { get; set; } =String.Empty;
    public string Discord { get; set; } =String.Empty;
        
    public string Nickname { get; set; } =String.Empty;
    public string PubgId { get; set; } = String.Empty;
    
    public User User { get; set; } = null!;
    public Guid UserId { get; set; }
    public MediaFile? ProfilePicture { get; set; }
}