using CleanArchitecture.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Profiles.Command.EditProfile;
[Authorize]
public class EditProfileCommand: IRequest<Unit>
{
    public string Name { get; set; } = String.Empty;
    public string Email { get; set; }= String.Empty;
    
    public string Telegram { get; set; }= String.Empty;
    public string Discord { get; set; }= String.Empty;
    public string Youtube { get; set; }= String.Empty;
    
    public string Nickname { get; set; }= String.Empty;
    public string PubgId { get; set; }= String.Empty;
}

public class EditProfileCommandHandler : IRequestHandler<EditProfileCommand, Unit>
{
    private IApplicationDbContext _context;

    private ICurrentUserService _currentUser;
    private IEmailSender _emailSender;

    public EditProfileCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser, IEmailSender emailSender)
    {
        _context = context;
        _currentUser = currentUser;
        _emailSender = emailSender;
    }

    public async Task<Unit> Handle(EditProfileCommand request, CancellationToken cancellationToken)
    {
        Profile profile = await _context.Profiles.Include(x=>x.User).FirstAsync(x => x.UserId == _currentUser.UserIdGuid, cancellationToken: cancellationToken);
        if (profile.Email != request.Email)
        {
            profile.User.EmailConfirmed = false;
        }
        profile.Name = request.Name;
        profile.Email = request.Email;
        profile.Telegram = request.Telegram;
        profile.Discord = request.Discord;
        profile.Youtube = request.Youtube;

        profile.Nickname = request.Nickname;
        profile.PubgId = request.PubgId;
        _context.Profiles.Update(profile);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    
        
    }
}