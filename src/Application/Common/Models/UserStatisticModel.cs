namespace CleanArchitecture.Application.Common.Models;

public class UserStatisticModel
{
    public Guid UserId { get; set; }
    public int Place { get; set; }
    public int Points { get; set; }
    public int Kills { get; set; }
}