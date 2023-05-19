using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Shop.Command.UpdateShopItem;
[EmployeeAuthorize("Admin")]
public class UpdateShopItemCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
    public ShopItemType Tag { get; set; }
    public string Title { get; set; } = String.Empty;
    public string Description { get; set; } = String.Empty;
    public string Specifications { get; set; } = String.Empty;
    public int Amount { get; set; }
    public int Price { get; set; }
}

public class UpdateShopItemCommandHandler : IRequestHandler<UpdateShopItemCommand, Unit>
{
    private IApplicationDbContext _context;

    public UpdateShopItemCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateShopItemCommand request, CancellationToken cancellationToken)
    {
        ShopItem shopItem = await _context.ShopItems.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken)
                            ?? throw new BadRequestException("Shop item not found");
        shopItem.Tag = request.Tag;
        shopItem.Title = request.Title;
        shopItem.Description = request.Description;
        shopItem.Specifications = request.Specifications;
        shopItem.Amount = request.Amount;
        shopItem.Price = request.Price;
        _context.ShopItems.Update(shopItem);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}