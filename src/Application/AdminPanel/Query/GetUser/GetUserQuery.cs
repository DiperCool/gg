using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArchitecture.Application.Common.DTOs;
using CleanArchitecture.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.AdminPanel.Query.GetUser;
[EmployeeAuthorize]
public class GetUserQuery : IRequest<UserDTO>
{
    public string? UserId { get; set; }
    public string? Nickname { get; set; }
}

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserDTO>
{
    private IApplicationDbContext _context;
    private IMapper _mapper;

    public GetUserQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<UserDTO> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var queryable = _context.Users.AsQueryable();
        if (!string.IsNullOrEmpty(request.UserId))
        {
            queryable = queryable.Where(x => x.Id == Guid.Parse(request.UserId));
        }

        if (!string.IsNullOrEmpty(request.Nickname))
        {
             queryable = queryable.Where(x => x.Profile.Nickname == request.Nickname);
        }

        return await queryable.ProjectTo<UserDTO>(_mapper.ConfigurationProvider)
                   .FirstOrDefaultAsync(cancellationToken: cancellationToken) ??
               throw new BadRequestException("User not found");
    }
}