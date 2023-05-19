using CleanArchitecture.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Profiles.Command.ConfirmEmail;

public class ConfirmEmailCommand: IRequest<Unit>
{
    public string Code { get; set; } = String.Empty;
}

public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, Unit>
{
    private IApplicationDbContext _context;
    
    public ConfirmEmailCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        var result = await _context.ConfirmEmails
                         .Where(x => x.Id == Guid.Parse(request.Code))
                         .Select(x => new { x.User }).FirstOrDefaultAsync(cancellationToken: cancellationToken) ??
                     throw new BadRequestException("Code doesn't exist");
        result.User.EmailConfirmed = true;
        _context.Users.Update(result.User);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}