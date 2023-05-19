using CleanArchitecture.Application.Teams.Query.GetInvitationCodeLink;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Teams.Query.GetManagerInvitationCodeLink;
[Authorize]
public class GetManagerInvitationCodeLinkQuery : IRequest<InvitationCodeModel>
{
    public Guid TeamId { get; set; }
}

public class GetManagerInvitationCodeLinkQueryHandler : IRequestHandler<GetManagerInvitationCodeLinkQuery, InvitationCodeModel>
{
    private IApplicationDbContext _context;
    private ICurrentUserService _current;

    public GetManagerInvitationCodeLinkQueryHandler(IApplicationDbContext context, ICurrentUserService current)
    {
        _context = context;
        _current = current;
    }

    public async Task<InvitationCodeModel> Handle(GetManagerInvitationCodeLinkQuery request, CancellationToken cancellationToken)
    {
        return new InvitationCodeModel()
        {
            Code = await _context.TeamUsers
                       .Where(x=>!x.Team.IsDeleted &&x.TeamId==request.TeamId && x.Teammate.UserId==_current.UserIdGuid)
                       .Select(x=>x.Team.ManagerInvitationCodeLink)
                       .FirstOrDefaultAsync(cancellationToken: cancellationToken) ?? 
                   throw new BadRequestException("You are not in this team")
        };
    }
}