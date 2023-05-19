using CleanArchitecture.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.AdminPanel.Command.DeleteUser;
[EmployeeAuthorize("Owner", "Admin")]
public class DeleteUserCommand: IRequest<Unit>
{
    public Guid UserId { get; set; }
}

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Unit>
{
    private ICurrentUserService _current;
    private IApplicationDbContext _context;

    public DeleteUserCommandHandler(ICurrentUserService current, IApplicationDbContext context)
    {
        _current = current;
        _context = context;
    }

    public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        User userToDel = await _context.Users.FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken: cancellationToken) ??
                         throw new BadRequestException("User doesn't exists");
        userToDel.IsDeleted = true;
        userToDel.DeletedById = _current.UserIdGuid;
        _context.Users.Update(userToDel);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}