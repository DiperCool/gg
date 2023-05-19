using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.Common.DTOs;

public class UserStatisticDTO : IMapFrom<UserStatistic>
{
    public int Kills { get; set; }
    public int Points { get; set; }
}