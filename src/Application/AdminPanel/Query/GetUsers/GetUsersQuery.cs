using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArchitecture.Application.Common.DTOs;
using CleanArchitecture.Domain.Entities;
using MediatR;

namespace CleanArchitecture.Application.AdminPanel.Query.GetUsers;
[EmployeeAuthorize]
public class GetUsersQuery : IRequest<PaginatedList<UserDTO>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; }= 25;
    public bool GetActive { get; set; } = false;
}

public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, PaginatedList<UserDTO>>
{
    private IApplicationDbContext _application;

    private IMapper _mapper;

    public GetUsersQueryHandler(IApplicationDbContext application, IMapper mapper)
    {
        _application = application;
        _mapper = mapper;
    }

    public Task<PaginatedList<UserDTO>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var query = _application.Users.AsQueryable();
        if (request.GetActive)
        {
            query = query.Where(x => x.ActiveUserLogging.UserLogging.Time.AddDays(14) >= DateTime.UtcNow);
        }
        return query.ProjectTo<UserDTO>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}