using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArchitecture.Application.Common.DTOs;
using CleanArchitecture.Application.Common.DTOs.UserProfiles;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Profiles.Query.GetProfile;
[Authorize]
public class GetProfileQuery: IRequest<PrivateProfileDTO>
{
    
}

public class GetProfileQueryHandler : IRequestHandler<GetProfileQuery, PrivateProfileDTO>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    private readonly ICurrentUserService _currentUser;

    public GetProfileQueryHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUser)
    {
        _context = context;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<PrivateProfileDTO> Handle(GetProfileQuery request, CancellationToken cancellationToken)
    {
        return await _context.Profiles.AsNoTracking()
            .Where(x => x.UserId == _currentUser.UserIdGuid)
            .ProjectTo<PrivateProfileDTO>(_mapper.ConfigurationProvider)
            .FirstAsync(cancellationToken: cancellationToken);
    }
}