using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArchitecture.Application.Common.DTOs;
using CleanArchitecture.Domain.Entities;
using MediatR;

namespace CleanArchitecture.Application.AdminPanel.Query.GetLogsByEmployeeId;
[EmployeeAuthorize]
public class GetLogsByEmployeeIdQuery : IRequest<PaginatedList<EmployeeLogDTO>>
{
    public Guid EmployeeId { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 50;
}

public class GetLogsByEmployeeIdQueryHandler : IRequestHandler<GetLogsByEmployeeIdQuery, PaginatedList<EmployeeLogDTO>>
{
    private IApplicationDbContext _context;
    private IMapper _mapper;

    public GetLogsByEmployeeIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public Task<PaginatedList<EmployeeLogDTO>> Handle(GetLogsByEmployeeIdQuery request, CancellationToken cancellationToken)
    {
        return _context.EmployeeLogs
            .Where(x=>x.EmployeeId == request.EmployeeId)
            .OrderByDescending(x => x.LoggedAt)
            .ProjectTo<EmployeeLogDTO>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}