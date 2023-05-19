using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Enums;
using MediatR;

namespace CleanArchitecture.Application.Shop.Command.CreateShopItem;

[EmployeeAuthorize("Admin")]
public class CreateShopItemCommand : IRequest<Guid>
{
    public ShopItemType Tag { get; set; }
    public string Title { get; set; } = String.Empty;
    public string Description { get; set; } = String.Empty;
    public string Specifications { get; set; } = String.Empty;
    public int Amount { get; set; }
    public int Price { get; set; }
}

public class CreateShopItemCommandHandler : IRequestHandler<CreateShopItemCommand, Guid>
{
    private IApplicationDbContext _context;
    private ICurrentUserService _current;

    public CreateShopItemCommandHandler(IApplicationDbContext context, ICurrentUserService current)
    {
        _context = context;
        _current = current;
    }

    public async Task<Guid> Handle(CreateShopItemCommand request, CancellationToken cancellationToken)
    {
        ShopItem shopItem = new()
        {
            Tag = request.Tag,
            Title = request.Title,
            Description = request.Description,
            Specifications = request.Specifications,
            Amount = request.Amount,
            Price = request.Price,
            CreatedAt = DateTime.UtcNow,
            CreatedById = _current.UserIdGuid
        };
        await _context.ShopItems.AddAsync(shopItem, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return shopItem.Id;
    }
}
