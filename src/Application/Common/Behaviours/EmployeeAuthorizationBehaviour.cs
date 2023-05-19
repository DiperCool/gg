using System.Reflection;
using CleanArchitecture.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Common.Behaviours;
public class EmployeeAuthorizationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IApplicationDbContext _applicationDbContext;

    public EmployeeAuthorizationBehaviour(ICurrentUserService currentUserService, IApplicationDbContext applicationDbContext)
    {
        _currentUserService = currentUserService;
        _applicationDbContext = applicationDbContext;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        var authorizeAttributes = request.GetType().GetCustomAttributes<EmployeeAuthorizeAttribute>();

        IEnumerable<EmployeeAuthorizeAttribute> employeeAuthorizeAttributes = authorizeAttributes.ToList();
        if (employeeAuthorizeAttributes.Any())
        {
            if (String.IsNullOrEmpty(_currentUserService.UserId))
            {
                throw new UnauthorizedAccessException();
            }
            
            Employee? employee = await _applicationDbContext.Employees.Include(x => x.Role)
                .FirstOrDefaultAsync(x => x.Id == _currentUserService.UserIdGuid && !x.IsDeleted, cancellationToken: cancellationToken);
            if (employee==null)
            {
                throw new UnauthorizedAccessException();
            }

            foreach(EmployeeAuthorizeAttribute authorizeAttribute in employeeAuthorizeAttributes)
            {
                if (authorizeAttribute.AllowedRoles.Count!=0&&authorizeAttribute.AllowedRoles.All(x => x != employee.Role.Role))
                {
                    throw new ForbiddenAccessException();
                }
            }
        }

        return await next();
    }
}