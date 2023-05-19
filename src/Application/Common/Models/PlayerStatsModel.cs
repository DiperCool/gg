namespace CleanArchitecture.Application.Common.Models;

public class PlayerStatsModel
{
    public Guid Id { get; set; }
    public int Place { get; set; }
    public int Points { get; set; }

    public int Kills { get; set; }
}