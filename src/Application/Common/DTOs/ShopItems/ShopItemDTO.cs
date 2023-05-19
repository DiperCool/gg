using CleanArchitecture.Application.Common.DTOs.Employee;
using CleanArchitecture.Domain.Enums;

namespace CleanArchitecture.Application.Common.DTOs.ShopItems;

public class ShopItemDTO: IMapFrom<Domain.Entities.ShopItem>
{
    public Guid Id { get; set; }
    public ShopItemType Tag { get; set; }
    public string Title { get; set; } = String.Empty;
    public string Description { get; set; } = String.Empty;
    public string Specifications { get; set; } = String.Empty;
    public int Amount { get; set; }
    public int AmountUsersPurchased { get; set; }
    public int Price { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid CreatedById { get; set; }
    public PublicEmployeeDTO CreatedBy { get; set; } = null!;

}