using CleanArchitecture.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Shop.Command.BuyShopItem;
[Authorize]
public class BuyShopItemCommand: IRequest<Unit>
{
    public Guid ShopItemId { get; set; }
}

public class BuyShopItemCommandHandler : IRequestHandler<BuyShopItemCommand, Unit>
{
    private IApplicationDbContext _context;
    private ICurrentUserService _current;

    public BuyShopItemCommandHandler(IApplicationDbContext context, ICurrentUserService current)
    {
        _context = context;
        _current = current;
    }

    public async Task<Unit> Handle(BuyShopItemCommand request, CancellationToken cancellationToken)
    {
        ShopItem shopItem = await _context.ShopItems.FirstOrDefaultAsync(x => x.Id == request.ShopItemId, cancellationToken: cancellationToken)
                            ?? throw new BadRequestException("Shop item not found");
        User user = await _context.Users.FirstAsync(x => x.Id == _current.UserIdGuid, cancellationToken: cancellationToken);
        if (shopItem.Price > user.Coins)
        {
            throw new BadRequestException("no money");
        }

        if (shopItem.Amount <= 0)
        {
            throw new BadRequestException("out of stock");
        }
        user.Coins -= shopItem.Price;
        shopItem.Amount -= 1;
        shopItem.AmountUsersPurchased += 1;
        ShopItemUser shopItemUser = new() { ShopItemId = shopItem.Id, UserId = user.Id };
        await _context.ShopItemUsers.AddAsync(shopItemUser, cancellationToken);
        _context.Users.Update(user);
        _context.ShopItems.Update(shopItem);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;

    }
}