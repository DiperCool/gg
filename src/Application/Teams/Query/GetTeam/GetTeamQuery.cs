using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArchitecture.Application.Common.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Teams.Query.GetTeam;

public class GetTeamQuery : IRequest<TeamDTO>
{
    public Guid TeamId { get; set; }
}

public class GetTeamQueryHandler : IRequestHandler<GetTeamQuery, TeamDTO>
{
    private IApplicationDbContext _context;
    private IMapper _mapper;

    public GetTeamQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<TeamDTO> Handle(GetTeamQuery request, CancellationToken cancellationToken)
    {
        return await _context.Teams.Where(x=>!x.IsDeleted &&x.Id==request.TeamId).ProjectTo<TeamDTO>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(cancellationToken: cancellationToken)
               ?? throw new BadRequestException("Team not found");
    }
}