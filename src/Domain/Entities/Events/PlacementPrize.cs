namespace CleanArchitecture.Domain.Entities.Events;

public class PlacementPrize
{
    public Guid Id { get; set; }
    public Guid PrizeObjId { get; set; }
    public Prize PrizeObj { get; set; } = null!;
    
    public int Number { get; set; }
    public int Prize { get; set; }
}