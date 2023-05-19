using CleanArchitecture.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.AdminPanel.Command.RestoreUser;
[EmployeeAuthorize("Owner", "Admin")]
public class RestoreUserCommand: IRequest<Unit>
{
    public Guid UserId { get; set; }
}

public class RestoreUserCommandHandler : IRequestHandler<RestoreUserCommand, Unit>
{
    private ICurrentUserService _current;
    private IApplicationDbContext _context;

    public RestoreUserCommandHandler(ICurrentUserService current, IApplicationDbContext context)
    {
        _current = current;
        _context = context;
    }
    public async Task<Unit> Handle(RestoreUserCommand request, CancellationToken cancellationToken)
    {
        User userToDel = await _context.Users.FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken: cancellationToken) ??
                         throw new BadRequestException("User doesn't exists");
        userToDel.IsDeleted = false;
        userToDel.DeletedById =null;
        _context.Users.Update(userToDel);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}