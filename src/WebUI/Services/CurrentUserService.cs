using System.Security.Claims;

using CleanArchitecture.Application.Common.Interfaces;

namespace CleanArchitecture.WebUI.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier)??String.Empty;
    public Guid UserIdGuid => String.IsNullOrEmpty(UserId)?Guid.Empty:new Guid(UserId);
}
