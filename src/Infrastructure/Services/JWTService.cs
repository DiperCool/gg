using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Infrastructure.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CleanArchitecture.Infrastructure.Services;
public class JWTService : IJWTService
{
    private JwtSettings JwtSettings;

    private IApplicationDbContext _context;

    public JWTService(IOptions<JwtSettings> jwtSettings, IApplicationDbContext context)
    {
        JwtSettings = jwtSettings.Value;
        _context = context;
    }

    public async Task<TokenResult> GenerateToken(Guid entityId, List<Claim> claims, bool user=true)
    {
        string tokenWritten = WriteToken(claims.ToArray());

        RefreshToken refreshToken = await CreateRefsRefreshToken(entityId, tokenWritten,user);

        return new()
        {
            AccessToken=tokenWritten,
            RefreshToken = refreshToken.Token
        };
    }

    private async Task<RefreshToken> CreateRefsRefreshToken(Guid entityId, string tokenWritten, bool user=true)
    {
        RefreshToken refreshToken = new()
        {
            JwtToken = tokenWritten,
            IsUsed = false,
            AddedDate = DateTime.UtcNow,
            ExpiryDate = DateTime.UtcNow.AddDays(5),
            IsRevoked = false,
            Token = RandomString(25) + Guid.NewGuid()
        };
        if (user)
        {
            refreshToken.UserId = entityId;
        }
        else
        {
            refreshToken.EmployeeId = entityId;
        }
        await _context.RefreshTokens.AddAsync(refreshToken);
        await _context.SaveChangesAsync(new CancellationToken());
        return refreshToken;
    }

    private string WriteToken(Claim[] claims)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSettings.Key));
        var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: JwtSettings.Issuer,
            audience: JwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(JwtSettings.ExpirationMinutes),
            signingCredentials: signIn);
        string tokenWritten = new JwtSecurityTokenHandler().WriteToken(token);
        return tokenWritten;
    }

    public bool VerifyToken(string token)
    {
        var jwtTokenHandler = new JwtSecurityTokenHandler();

        var tokenValidationParameters = new TokenValidationParameters {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            ValidIssuer =JwtSettings.Issuer,
            ValidAudience = JwtSettings.Audience,
            IssuerSigningKey = new
                SymmetricSecurityKey
                (Encoding.UTF8.GetBytes
                    (JwtSettings.Key))
        };

        try
        {
            var principal = jwtTokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
            if(validatedToken is JwtSecurityToken jwtSecurityToken)
            {
                return jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
            }

            return false;
        }
        catch (Exception e)
        {
            return false;
        }

    }
    private string RandomString(int length)
    {
        var random = new Random();
        var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}