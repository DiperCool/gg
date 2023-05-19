using CleanArchitecture.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Profiles.Command.ResendEmailConfirmation;

[Authorize]
public class ResendEmailConfirmationCommand: IRequest<Unit>
{
    
}

public class ResendEmailConfirmationCommandHandler : IRequestHandler<ResendEmailConfirmationCommand, Unit>
{
    private IApplicationDbContext _context;
    private ICurrentUserService _currentUser;
    readonly IEmailSender _emailSender;

    readonly IEmailTemplate _emailTemplate;

    public ResendEmailConfirmationCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser, IEmailSender emailSender, IEmailTemplate emailTemplate)
    {
        _context = context;
        _currentUser = currentUser;
        _emailSender = emailSender;
        _emailTemplate = emailTemplate;
    }

    public async Task<Unit> Handle(ResendEmailConfirmationCommand request, CancellationToken cancellationToken)
    {
        if (await _context.Users.AnyAsync(x => x.Id == _currentUser.UserIdGuid && x.EmailConfirmed, cancellationToken: cancellationToken))
        {
            throw new BadRequestException("Email already confirmed");
        }
        var user = await _context.Users.Select(x=>new
        {
            x.Id, x.Profile.Email
        }).FirstAsync(x => x.Id == _currentUser.UserIdGuid, cancellationToken: cancellationToken);

        await _emailSender.SendEmailConfirmation(user.Id, user.Email);
        return Unit.Value;
    }
}