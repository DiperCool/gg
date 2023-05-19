using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.AdminPanel.Command.GiveShadowBan;
[EmployeeAuthorize("Owner", "Admin")]

public class GiveShadowBanCommand: IRequest<Unit>
{
    public Guid UserId { get; set; }
    public DateTime To { get; set; }
}

public class GiveShadowBanCommandHandler : IRequestHandler<GiveShadowBanCommand, Unit>
{
    private IApplicationDbContext _context;
    private ICurrentUserService _current;
    private IEmployeeLogService _log;
    public GiveShadowBanCommandHandler(IApplicationDbContext context, ICurrentUserService current, IEmployeeLogService log)
    {
        _context = context;
        _current = current;
        _log = log;
    }

    public async Task<Unit> Handle(GiveShadowBanCommand request, CancellationToken cancellationToken)
    {
        ShadowBan ban =await _context.ShadowBans.FirstOrDefaultAsync(x => x.User.Id == request.UserId,
            cancellationToken: cancellationToken) ?? throw new BadRequestException("User not found");
        ban.To = request.To;
        ban.EmployeeId = _current.UserIdGuid;
        _context.ShadowBans.Update(ban);
        await _context.SaveChangesAsync(cancellationToken);
        await _log.Log(_current.UserIdGuid, LogsEnum.ShadowBan, _current.UserIdGuid, ban.UserId);
        return Unit.Value;
    }
}