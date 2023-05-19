using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArchitecture.Application.Common.DTOs.ShopItems;
using MediatR;

namespace CleanArchitecture.Application.Shop.Query.GetShopItems;

public class GetShopItemsQuery : IRequest<PaginatedList<PublicShopItemDTO>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 25;
}

public class GetShopItemsQueryHandler : IRequestHandler<GetShopItemsQuery, PaginatedList<PublicShopItemDTO>>
{
    private IApplicationDbContext _context;
    private IMapper _mapper;
    public GetShopItemsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<PublicShopItemDTO>> Handle(GetShopItemsQuery request, CancellationToken cancellationToken)
    {
        return await _context.ShopItems.ProjectTo<PublicShopItemDTO>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}