using CleanArchitecture.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Authenticate.Command.Register
{
    public class RegisterUserCommand: IRequest<TokenResult>
    {
        public string Email { get ;set; }=String.Empty;
        public string Name { get; set; } = String.Empty;
        public string Password { get; set; }=String.Empty;
        public string ConfirmPassword { get ;set; } = String.Empty;
        public string Nickname { get; set; } =String.Empty;
        public string PubgId { get; set; } = String.Empty;
    }

    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, TokenResult>
    {
        IJWTService _IJWTService;
        IApplicationDbContext _context;
        IHashPassword _hashPassword;
        readonly IEmailSender _emailSender;

        readonly IEmailTemplate _emailTemplate;

        public RegisterUserCommandHandler(IJWTService ijwtService, IApplicationDbContext context, IHashPassword hashPassword, IEmailSender emailSender, IEmailTemplate emailTemplate)
        {
            _IJWTService = ijwtService;
            _context = context;
            _hashPassword = hashPassword;
            _emailSender = emailSender;
            _emailTemplate = emailTemplate;
        }

        public async Task<TokenResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            if (await _context.Profiles.AnyAsync(x => x.PubgId == request.PubgId && x.User.Ban.To >= DateTime.UtcNow, cancellationToken: cancellationToken))
            {
                throw new BadRequestException();
            }
            Profile profile = new()
            {
                Email = request.Email,Name = request.Name, Nickname = request.Nickname, PubgId = request.PubgId,Login = request.Name
            };
            User user = new()
            {
                RegisteredAt = DateTime.UtcNow,
                Password= _hashPassword.Hash(request.Password),
                Profile = profile,
                ActiveUserLogging = new ActiveUserLogging(),
                Ban = new Ban(),
                ShadowBan = new ShadowBan(),
                Statistic = new UserStatistic()
                {
                    Kills = 0
                },
                Coins = 0
            };
            UserLogging ul = new()
            {
                Time = DateTime.UtcNow,
                User = user
            };
            user.UserLogging.Add(ul);
            user.ActiveUserLogging.UserLogging = ul;
            await _context.Users.AddAsync(user, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            await _emailSender.SendEmailConfirmation(user.Id, request.Email);

            return await _IJWTService.GenerateToken(user.Id, DefaultClaims.GetUserClaims(user));
        }
    }
}