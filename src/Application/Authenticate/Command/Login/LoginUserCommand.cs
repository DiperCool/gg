using CleanArchitecture.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Authenticate.Command.Login
{
    public class LoginUserCommand: IRequest<TokenResult>
    {
        public string Email { get; set; }= string.Empty;
        public string Password { get ;set; }=string.Empty;
    }
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, TokenResult>
    {
        IJWTService _IJWTService;
        IApplicationDbContext _context;
        IHashPassword _hashPassword;

        public LoginUserCommandHandler(IJWTService ijwtService, IApplicationDbContext context, IHashPassword hashPassword)
        {
            _IJWTService = ijwtService;
            _context = context;
            _hashPassword = hashPassword;
        }

        public async Task<TokenResult> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            User user = await _context.Users.Include(x=>x.Profile
            ).Include(x=>x.ActiveUserLogging)
                .FirstOrDefaultAsync(x=>x.Profile.Email==request.Email&&x.Password== _hashPassword.Hash(request.Password) && !x.IsDeleted && (x.Ban.To ==null || x.Ban.To <= DateTime.UtcNow), cancellationToken: cancellationToken)?? throw new BadRequestException("Email or password is incorrect");
            UserLogging ul = new() { UserId = user.Id, Time = DateTime.UtcNow };
            user.ActiveUserLogging.UserLogging = ul;
            await _context.UserLogging.AddAsync(ul, cancellationToken);
            _context.Users.Update(user);
            await _context.SaveChangesAsync(cancellationToken);
            return await _IJWTService.GenerateToken(user.Id, DefaultClaims.GetUserClaims(user));
        }
    }
}