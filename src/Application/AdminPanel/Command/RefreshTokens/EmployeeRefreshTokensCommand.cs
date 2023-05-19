using CleanArchitecture.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.AdminPanel.Command.RefreshTokens;

public class EmployeeRefreshTokensCommand: IRequest<TokenResult>
{
    public string Token { get; set; } = String.Empty;
    public string RefreshToken { get; set; } = String.Empty;
}


public class EmployeeRefreshTokensCommandHandler : IRequestHandler<EmployeeRefreshTokensCommand, TokenResult>
{
    private IApplicationDbContext _context;
    private IJWTService _jwtService;

    public EmployeeRefreshTokensCommandHandler(IApplicationDbContext context, IJWTService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }
    public async Task<TokenResult> Handle(EmployeeRefreshTokensCommand request, CancellationToken cancellationToken)
    {
        if (!_jwtService.VerifyToken(request.Token))
        {
            throw new BadRequestException("Invalid token");
        }

        RefreshToken storedRefreshToken =
            await _context.RefreshTokens
                .Include(x=>x.Employee)
                    .ThenInclude(x=>x.Role)
                .FirstOrDefaultAsync(x => x.Token == request.RefreshToken,
                cancellationToken: cancellationToken) ?? throw new BadRequestException("Refresh token doesn't exist");
        if (storedRefreshToken.Employee == null)
        {
            throw new BadRequestException("You are not an employee");
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
        storedRefreshToken.IsUsed = true;
        _context.RefreshTokens.Update(storedRefreshToken);
        await _context.SaveChangesAsync(cancellationToken);
        return await _jwtService.GenerateToken(storedRefreshToken.Employee!.Id, DefaultClaims.GetEmployeeClaims(storedRefreshToken.Employee), user:false);
    }
}