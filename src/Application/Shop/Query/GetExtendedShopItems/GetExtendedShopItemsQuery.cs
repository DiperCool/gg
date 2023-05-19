using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArchitecture.Application.Common.DTOs.ShopItems;
using CleanArchitecture.Application.Shop.Query.GetShopItems;
using MediatR;

namespace CleanArchitecture.Application.Shop.Query.GetExtendedShopItems;
[EmployeeAuthorize]
public class GetExtendedShopItemsQuery : IRequest<PaginatedList<ShopItemDTO>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 25;
}

public class GetExtendedShopItemsQueryHandler : IRequestHandler<GetExtendedShopItemsQuery, PaginatedList<ShopItemDTO>>
{
    private IApplicationDbContext _context;
    private IMapper _mapper;
    public GetExtendedShopItemsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<ShopItemDTO>> Handle(GetExtendedShopItemsQuery request, CancellationToken cancellationToken)
    {
        return await _context.ShopItems.ProjectTo<ShopItemDTO>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}