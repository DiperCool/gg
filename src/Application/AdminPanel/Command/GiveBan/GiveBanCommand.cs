using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.AdminPanel.Command.GiveBan;
[EmployeeAuthorize("Owner", "Admin")]
public class GiveBanCommand : IRequest<Unit>
{
    public Guid UserId { get; set; }
    public DateTime To { get; set; }
}

public class GiveBanCommandHandler : IRequestHandler<GiveBanCommand, Unit>
{
    private ICurrentUserService _current;
    private IApplicationDbContext _context;
    private IEmployeeLogService _log;
    public GiveBanCommandHandler(ICurrentUserService current, IApplicationDbContext context, IEmployeeLogService log)
    {
        _current = current;
        _context = context;
        _log = log;
    }

    public async Task<Unit> Handle(GiveBanCommand request, CancellationToken cancellationToken)
    {
        Ban ban =await _context.Bans.FirstOrDefaultAsync(x => x.User.Id == request.UserId,
            cancellationToken: cancellationToken) ?? throw new BadRequestException("User not found");
        ban.To = request.To;
        ban.EmployeeId = _current.UserIdGuid;
        _context.Bans.Update(ban);
        await _context.SaveChangesAsync(cancellationToken);
        await _log.Log(_current.UserIdGuid, LogsEnum.Ban, _current.UserIdGuid, ban.UserId);
        return Unit.Value;
    }
}