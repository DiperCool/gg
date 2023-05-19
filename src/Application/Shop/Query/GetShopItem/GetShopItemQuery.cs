using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArchitecture.Application.Common.DTOs.ShopItems;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Shop.Query.GetShopItem;

public class GetShopItemQuery : IRequest<PublicShopItemDTO>
{
    public Guid Id { get; set; }
}

public class GetShopItemQueryHandler : IRequestHandler<GetShopItemQuery, PublicShopItemDTO>
{
    private IApplicationDbContext _context;
    private IMapper _mapper;
    public GetShopItemQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PublicShopItemDTO> Handle(GetShopItemQuery request, CancellationToken cancellationToken)
    {
        return await _context.ShopItems.Where(x=>x.Id==request.Id).ProjectTo<PublicShopItemDTO>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(cancellationToken: cancellationToken)
            ?? throw new BadRequestException("Shop item not found");
    }
}