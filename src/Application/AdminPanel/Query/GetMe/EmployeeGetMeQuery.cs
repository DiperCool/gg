using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArchitecture.Application.Common.DTOs;
using CleanArchitecture.Application.Common.DTOs.Employee;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.EmployeeProfiles;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.AdminPanel.Query.GetMe;
[EmployeeAuthorize]
public class EmployeeGetMeQuery : IRequest<EmployeeDTO>
{
   
}

public class EmployeeGetMeQueryHandler : IRequestHandler<EmployeeGetMeQuery, EmployeeDTO>
{
    private IApplicationDbContext _application;
    private ICurrentUserService _current;
    private IMapper _mappper;

    public EmployeeGetMeQueryHandler(IApplicationDbContext application, ICurrentUserService current, IMapper mappper)
    {
        _application = application;
        _current = current;
        _mappper = mappper;
    }

    public async Task<EmployeeDTO> Handle(EmployeeGetMeQuery request, CancellationToken cancellationToken)
    {
        Employee employee = await _application.Employees.AsNoTracking()
            .Include(x=>x.Role)
            .Include(x=>x.CreatedEmployees)
                .ThenInclude(x=>x.Profile)
            .Include(x=>x.CreatedEmployees)
                .ThenInclude(x=>x.Role)
            .Include(x=>x.Profile)
                .ThenInclude(x=>((OrganizerProfile)x).Logo)
            .Include(x=>x.Profile)
                .ThenInclude(x=>((OrganizerProfile)x).CreatedEvents)
            .Include(x=>x.Profile)
                .ThenInclude(x=>((NewsEditorProfile)x).CreatedNews)
            .Include(x=>x.Loggings)
            .Include(x=>x.DeletedUsers)
                .ThenInclude(x=>x.Profile)
            .Include(x=>x.DeletedEmployees)
                .ThenInclude(x=>x.Role)
            .Include(x=>x.DeletedEmployees)
                .ThenInclude(x=>x.Profile)
            .FirstAsync(x => x.Id == _current.UserIdGuid, cancellationToken: cancellationToken);
        return _mappper.Map<Employee, EmployeeDTO>(employee);

    }
}