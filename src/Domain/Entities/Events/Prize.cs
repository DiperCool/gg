namespace CleanArchitecture.Domain.Entities.Events;

public class Prize
{
    public Guid Id { get; set; }

    public Event Event { get; set; } = null!;
    public string EventId { get; set; } = String.Empty;
    
    public int Pool { get; set; }
    public int PrizePerKill { get; set; }
    public List<PlacementPrize> PlacementPrizes { get; set; } = new();
}