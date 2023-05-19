using CleanArchitecture.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Authenticate.Command.RefreshTokens;

public class RefreshTokenCommand : IRequest<TokenResult>
{
    public string Token { get; set; } = String.Empty;
    public string RefreshToken { get; set; } = String.Empty;
}

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, TokenResult>
{
    private IApplicationDbContext _context;
    private IJWTService _jwtService;

    public RefreshTokenCommandHandler(IApplicationDbContext context, IJWTService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }

    public async Task<TokenResult> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        if (!_jwtService.VerifyToken(request.Token))
        {
            throw new BadRequestException("Invalid token");
        }

        RefreshToken storedRefreshToken =
            await _context.RefreshTokens
                .Include(x=>x.User)
                    .ThenInclude(x=>x.Profile)
                .Include(x=>x.User)
                    .ThenInclude(x=>x.ActiveUserLogging)
                .FirstOrDefaultAsync(x => x.Token == request.RefreshToken,
                cancellationToken: cancellationToken) ?? throw new BadRequestException("Refresh token doesn't exist");
        if (storedRefreshToken.User == null)
        {
            throw new BadRequestException("You are not an user");
        }
        if(DateTime.UtcNow > storedRefreshToken.ExpiryDate)
        {
            throw new BadRequestException("Refresh token has expired, user needs to re-login");
        }
        if(storedRefreshToken.IsUsed)
        {
            throw new BadRequestException("Refresh token has been used");

        }
        if(storedRefreshToken.IsRevoked)
        {
            throw new BadRequestException("Token has been revoked");
        }

        if (storedRefreshToken.JwtToken != request.Token)
        {
            throw new BadRequestException("The token doesn't match the saved token");
        }

        UserLogging userLogging = new() { Time = DateTime.UtcNow, RefreshToken = true };
        storedRefreshToken.IsUsed = true;
        storedRefreshToken.User.UserLogging.Add(userLogging);
        storedRefreshToken.User.ActiveUserLogging.UserLogging = userLogging;
        _context.RefreshTokens.Update(storedRefreshToken);
        await _context.SaveChangesAsync(cancellationToken);
        return await _jwtService.GenerateToken(storedRefreshToken.User!.Id, DefaultClaims.GetUserClaims(storedRefreshToken.User));
    }
}