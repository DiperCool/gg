using System.Security.Claims;
using CleanArchitecture.Domain.Entities;
using Microsoft.IdentityModel.JsonWebTokens;

namespace CleanArchitecture.Application.Common.Security;

public static class DefaultClaims
{
    public static List<Claim> GetUserClaims(User user)
    {
       return new List<Claim>() {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.UniqueName, user.Profile.Email),
        };
    }
    public static List<Claim> GetEmployeeClaims(Employee employee)
    {
        return new List<Claim>() {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
            new(JwtRegisteredClaimNames.Sub, employee.Id.ToString()),
            new("Role", employee.Role.Role),
            new(JwtRegisteredClaimNames.UniqueName, employee.Nickname),
        };
    }
}