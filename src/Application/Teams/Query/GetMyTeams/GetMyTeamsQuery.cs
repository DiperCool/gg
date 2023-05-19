using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArchitecture.Application.Common.DTOs;
using MediatR;

namespace CleanArchitecture.Application.Teams.Query.GetMyTeams;

[Authorize]
public class GetMyTeamsQuery : IRequest<List<TeamDTO>>
{
    
}

public class GetMyTeamsQueryHandler : IRequestHandler<GetMyTeamsQuery, List<TeamDTO>>
{
    private IApplicationDbContext _context;
    private ICurrentUserService _current;
    private IMapper _mapper;

    public GetMyTeamsQueryHandler(IApplicationDbContext context, ICurrentUserService current, IMapper mapper)
    {
        _context = context;
        _current = current;
        _mapper = mapper;
    }

    public async Task<List<TeamDTO>> Handle(GetMyTeamsQuery request, CancellationToken cancellationToken)
    {
        return await _context.TeamUsers.Where(x => !x.Team.IsDeleted &&x.Teammate.User.Id == _current.UserIdGuid).Select(x => x.Team)
            .ProjectToListAsync<TeamDTO>(_mapper.ConfigurationProvider);
    }
}