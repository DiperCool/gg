using CleanArchitecture.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Authenticate.Command.GenerateRestorePassword;

public class GenerateRestorePasswordCommand: IRequest<Unit>
{
    public string Email { get; set; } = String.Empty;
}

public class GenerateRestorePasswordCommandHandler : IRequestHandler<GenerateRestorePasswordCommand, Unit>
{
    readonly IApplicationDbContext _context;
    readonly IEmailSender _emailSender;

    readonly IEmailTemplate _emailTemplate;

    public GenerateRestorePasswordCommandHandler(IApplicationDbContext context, IEmailSender emailSender, IEmailTemplate emailTemplate)
    {
        _context = context;
        _emailSender = emailSender;
        _emailTemplate = emailTemplate;
    }

    public async Task<Unit> Handle(GenerateRestorePasswordCommand request, CancellationToken cancellationToken)
    {
        User user = await _context.Users.FirstAsync(x => x.Profile.Email == request.Email, cancellationToken: cancellationToken);
        Domain.Entities.RestorePassword restorePassword = new()
        {
            User = user, CreatedAt = DateTime.UtcNow, ExpiresAt = DateTime.UtcNow.AddMinutes(15),
            IsRestored = false
        };
        await _context.RestorePasswords.AddAsync(restorePassword, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        await Task.Run(async () =>
        {
            string content = _emailTemplate.GetTemplate("restorePassword")
                .Replace("{link}", restorePassword.Id.ToString());
            await _emailSender.Send(content, "Restore password", request.Email);
        }, cancellationToken);
        return Unit.Value;
    }
}