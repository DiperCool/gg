namespace CleanArchitecture.Domain.Entities;

public class ShopItemUser
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public Guid ShopItemId { get; set; }
    public ShopItem ShopItem { get; set; } = null!;
}