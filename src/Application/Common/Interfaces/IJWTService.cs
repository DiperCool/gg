using System.Security.Claims;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.Common.Interfaces
{
    public interface IJWTService
    {
        Task<TokenResult> GenerateToken(Guid entityId, List<Claim> claims, bool user=true);
        bool VerifyToken(string token);
    }
}