using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.AdminPanel.Command.UnShadowBan;
[EmployeeAuthorize("Owner", "Admin")]
public class UnShadowBanCommand: IRequest<Unit>
{
    public Guid UserId { get; set; }
}

public class UnShadowBanCommandHandler : IRequestHandler<UnShadowBanCommand, Unit>
{
    private IApplicationDbContext _context;
    private ICurrentUserService _current;
    private IEmployeeLogService _log;
    public UnShadowBanCommandHandler(IApplicationDbContext context, ICurrentUserService current, IEmployeeLogService log)
    {
        _context = context;
        _current = current;
        _log = log;
    }

    public async Task<Unit> Handle(UnShadowBanCommand request, CancellationToken cancellationToken)
    {
        ShadowBan ban =await _context.ShadowBans.FirstOrDefaultAsync(x => x.User.Id == request.UserId,
            cancellationToken: cancellationToken) ?? throw new BadRequestException("User not found");
        ban.To = null;
        ban.EmployeeId = null;
        _context.ShadowBans.Update(ban);
        await _context.SaveChangesAsync(cancellationToken);
        await _log.Log(_current.UserIdGuid, LogsEnum.UnShadowBan, _current.UserIdGuid, request.UserId);
        return Unit.Value;
    }
}