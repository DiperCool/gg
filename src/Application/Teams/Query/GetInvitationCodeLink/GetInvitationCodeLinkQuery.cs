using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Teams.Query.GetInvitationCodeLink;
[Authorize]
public class GetInvitationCodeLinkQuery : IRequest<InvitationCodeModel>
{
    public Guid TeamId { get; set; }
}

public class GetInvitationCodeLinkQueryHandler : IRequestHandler<GetInvitationCodeLinkQuery, InvitationCodeModel>
{
    private IApplicationDbContext _context;
    private ICurrentUserService _current;

    public GetInvitationCodeLinkQueryHandler(IApplicationDbContext context, ICurrentUserService current)
    {
        _context = context;
        _current = current;
    }

    public async Task<InvitationCodeModel> Handle(GetInvitationCodeLinkQuery request, CancellationToken cancellationToken)
    {
        return new InvitationCodeModel()
        {
            Code = await _context.TeamUsers
                .Where(x=>!x.Team.IsDeleted && x.TeamId==request.TeamId && x.Teammate.UserId==_current.UserIdGuid)
                .Select(x=>x.Team.InvitationCodeLink)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken) ?? 
                   throw new BadRequestException("You are not in this team")
        };
    }
}