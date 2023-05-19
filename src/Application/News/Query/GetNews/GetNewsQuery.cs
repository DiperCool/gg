using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArchitecture.Application.Common.DTOs;
using CleanArchitecture.Domain.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.News.Query.GetNews;

public class GetNewsQuery : IRequest<PaginatedList<NewsDTO>>
{
    public int PageNumber { get; set; } = 1;
    public NewsType? Type { get; set; }
}

public class GetNewsQueryHandler : IRequestHandler<GetNewsQuery, PaginatedList<NewsDTO>>
{
    private IApplicationDbContext _context;

    private IMapper _mapper;

    public GetNewsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<NewsDTO>> Handle(GetNewsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.News.AsNoTracking();
        if (request.Type != null)
        {
            query=query.Where(x => x.Type == request.Type);
        }
        return await query
            .Where(x => !x.IsDeleted)
            .ProjectTo<NewsDTO>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, 25);
    }
}