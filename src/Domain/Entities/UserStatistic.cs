namespace CleanArchitecture.Domain.Entities;

public class UserStatistic
{
    public Guid Id { get; set; }
    public int Kills { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}