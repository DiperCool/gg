using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.AdminPanel.Command.Unban;

[EmployeeAuthorize("Owner", "Admin")]
public class UnbanCommand : IRequest<Unit>
{
    public Guid UserId { get; set; }
}

public class UnbanCommandHandler : IRequestHandler<UnbanCommand, Unit>
{
    private ICurrentUserService _current;
    private IApplicationDbContext _context;
    private IEmployeeLogService _log;
    public UnbanCommandHandler(ICurrentUserService current, IApplicationDbContext context, IEmployeeLogService log)
    {
        _current = current;
        _context = context;
        _log = log;
    }

    public async Task<Unit> Handle(UnbanCommand request, CancellationToken cancellationToken)
    {
        Ban ban =await _context.Bans.FirstOrDefaultAsync(x => x.User.Id == request.UserId,
            cancellationToken: cancellationToken) ?? throw new BadRequestException("User not found");
        ban.To = null;
        ban.EmployeeId = null;
        _context.Bans.Update(ban);
        await _context.SaveChangesAsync(cancellationToken);
        await _log.Log(_current.UserIdGuid, LogsEnum.Unban, _current.UserIdGuid, request.UserId);
        return Unit.Value;
    }
}

