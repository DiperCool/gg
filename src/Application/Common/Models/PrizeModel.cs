using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.Common.Models;

public class PrizeModel
{
    public int Pool { get; set; }
    public int PrizePerKill { get; set; }
    public List<PlacementPrizeModel> PlacementPrizes { get; set; } = new();
}