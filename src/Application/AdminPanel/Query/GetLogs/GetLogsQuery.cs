using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArchitecture.Application.Common.DTOs;
using CleanArchitecture.Domain.Entities;
using MediatR;

namespace CleanArchitecture.Application.AdminPanel.Query.GetLogs;
[EmployeeAuthorize]
public class GetLogsQuery: IRequest<PaginatedList<EmployeeLogDTO>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 50;
}

public class GetLogsQueryHandler : IRequestHandler<GetLogsQuery, PaginatedList<EmployeeLogDTO>>
{
    private IApplicationDbContext _context;
    private IMapper _mapper;

    public GetLogsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public Task<PaginatedList<EmployeeLogDTO>> Handle(GetLogsQuery request, CancellationToken cancellationToken)
    {
        return _context.EmployeeLogs
            .OrderByDescending(x => x.LoggedAt)
            .ProjectTo<EmployeeLogDTO>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}