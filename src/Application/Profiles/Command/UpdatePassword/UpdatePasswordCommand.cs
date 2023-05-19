using CleanArchitecture.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Profiles.Command.UpdatePassword;
[Authorize]
public class UpdatePasswordCommand : IRequest<Unit>
{
    public string CurrentPassword { get;set; } = String.Empty;
    public string NewPassword { get; set; } =String.Empty;
    public string ConfirmNewPassword { get;set; } =String.Empty;
}

public class UpdatePasswordCommandHandler : IRequestHandler<UpdatePasswordCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    private readonly ICurrentUserService _currentUser;

    private readonly IHashPassword _hashPassword;

    public UpdatePasswordCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser, IHashPassword hashPassword)
    {
        _context = context;
        _currentUser = currentUser;
        _hashPassword = hashPassword;
    }

    public async Task<Unit> Handle(UpdatePasswordCommand request, CancellationToken cancellationToken)
    {
        string hashedPassword = _hashPassword.Hash(request.CurrentPassword);
        User user = await _context.Users.FirstOrDefaultAsync(x =>
            x.Id == _currentUser.UserIdGuid && x.Password == hashedPassword, cancellationToken: cancellationToken) ?? throw new BadRequestException("Password doesn't match current password");
        user.Password = _hashPassword.Hash(request.NewPassword);
        _context.Users.Update(user);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}