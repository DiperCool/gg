using CleanArchitecture.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Authenticate.Command.RestorePassword;

public class RestorePasswordCommand: IRequest<Unit>
{
    public Guid Code { get; set; }
    public string Password { get; set; } =String.Empty;
    public string ConfirmPassword { get ;set; } = String.Empty;
}

public class RestorePasswordCommandHandler : IRequestHandler<RestorePasswordCommand, Unit>
{
    private IApplicationDbContext _context;

    private IHashPassword _hash;

    public RestorePasswordCommandHandler(IApplicationDbContext context, IHashPassword hash)
    {
        _context = context;
        _hash = hash;
    }

    public async Task<Unit> Handle(RestorePasswordCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.RestorePassword rs = await _context.RestorePasswords.Include(x=>x.User).FirstAsync(x => x.Id == request.Code, cancellationToken: cancellationToken);
        if (DateTime.UtcNow > rs.ExpiresAt)
        {
            throw new BadRequestException("Expired");
        }

        if (rs.IsRestored)
        {
            throw new BadRequestException("Already restored by this code");
        }

        rs.User.Password = _hash.Hash(request.Password);
        rs.IsRestored = true;
        _context.RestorePasswords.Update(rs);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}