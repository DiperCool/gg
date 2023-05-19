using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArchitecture.Application.Common.DTOs;
using CleanArchitecture.Application.News.Query.GetNews;
using CleanArchitecture.Domain.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.News.Query.GetNewsByNewsEditorId;

public class GetNewsByNewsEditorIdQuery : IRequest<PaginatedList<NewsDTO>>
{
    public int PageNumber { get; set; } = 1;
    public NewsType? Type { get; set; }
    public Guid NewsEditorId { get; set; }
}


public class GetNewsByNewsEditorIdQueryHandler : IRequestHandler<GetNewsByNewsEditorIdQuery, PaginatedList<NewsDTO>>
{
    private IApplicationDbContext _context;

    private IMapper _mapper;

    public GetNewsByNewsEditorIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<NewsDTO>> Handle(GetNewsByNewsEditorIdQuery request, CancellationToken cancellationToken)
    {
        var query = _context.News.AsNoTracking();
        if (request.Type != null)
        {
            query = query.Where(x => x.Type == request.Type);
        }
        return await query
            .Where(x => !x.IsDeleted && x.CreatorId==request.NewsEditorId)
            .ProjectTo<NewsDTO>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, 25);
    }
}