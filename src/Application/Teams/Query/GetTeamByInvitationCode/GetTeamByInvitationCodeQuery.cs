using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArchitecture.Application.Common.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Teams.Query.GetTeamByInvitationCode;

public class GetTeamByInvitationCodeQuery : IRequest<TeamDTO>
{
   public string Code = String.Empty;
}

public class GetTeamByInvitationCodeQueryHandler : IRequestHandler<GetTeamByInvitationCodeQuery, TeamDTO>
{
    private IApplicationDbContext _context;
    private IMapper _mapper;

    public GetTeamByInvitationCodeQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<TeamDTO> Handle(GetTeamByInvitationCodeQuery request, CancellationToken cancellationToken)
    {
        return await _context.Teams.Where(x=>!x.IsDeleted &&x.InvitationCodeLink==request.Code || x.ManagerInvitationCodeLink==request.Code).ProjectTo<TeamDTO>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(cancellationToken: cancellationToken) ?? throw new BadRequestException("Team not found");
    }
}