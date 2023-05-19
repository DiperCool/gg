using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.AdminPanel.Command.GiveCoins;
[EmployeeAuthorize("Organizer", "Admin", "Owner")]
public class GiveCoinsCommand : IRequest<Unit>
{
    public Guid UserId { get; set; }
    public int Coins { get; set; }
}

public class GiveCoinsCommandHandler : IRequestHandler<GiveCoinsCommand, Unit>
{
    private IApplicationDbContext _dbContext;
    private ICurrentUserService _currentUserService;
    private IEmployeeLogService _employeeLogService;

    public GiveCoinsCommandHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService, IEmployeeLogService employeeLogService)
    {
        _dbContext = dbContext;
        _currentUserService = currentUserService;
        _employeeLogService = employeeLogService;
    }

    public async Task<Unit> Handle(GiveCoinsCommand request, CancellationToken cancellationToken)
    {
        User user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken: cancellationToken) ??
                    throw new BadRequestException("User not found");
        user.Coins += request.Coins;
        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync(cancellationToken);
        await _employeeLogService.Log(_currentUserService.UserIdGuid, LogsEnum.GiveCoins, _currentUserService.UserIdGuid,
            request.Coins, request.UserId);
        return Unit.Value;
    }
}