using System.Reflection;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Common.Behaviours;
public class AuthorizationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IApplicationDbContext _applicationDbContext;

    public AuthorizationBehaviour(ICurrentUserService currentUserService, IApplicationDbContext applicationDbContext)
    {
        _currentUserService = currentUserService;
        _applicationDbContext = applicationDbContext;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        var authorizeAttributes = request.GetType().GetCustomAttributes<AuthorizeAttribute>();

        if (authorizeAttributes.Any())
        {
            if (String.IsNullOrEmpty(_currentUserService.UserId))
            {
                throw new UnauthorizedAccessException();
            }

            if (!await _applicationDbContext.Users.AnyAsync(x => x.Id == _currentUserService.UserIdGuid && !x.IsDeleted && (x.Ban.To ==null || x.Ban.To <= DateTime.UtcNow),
                    cancellationToken: cancellationToken))
            {
                throw new UnauthorizedAccessException();
            }
        }

        // User is authorized / authorization not required
        return await next();
    }
}