using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.EmployeeProfiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Infrastructure.Persistence;

public class ApplicationDbContextInitialiser
{
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly ApplicationDbContext _context;
    private readonly IHashPassword _hashPassword;

    public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger, ApplicationDbContext context, IHashPassword hashPassword)
    {
        _logger = logger;
        _context = context;
        _hashPassword = hashPassword;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            if((await _context.Database.GetPendingMigrationsAsync()).Any())
            {
                await _context.Database.MigrateAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    private async Task TrySeedAsync()
    {
        await SeedRoles();
        await SeedEmployee();
    }

    private async Task SeedEmployee()
    {
        Employee moderator = new()
        {
            Nickname = "moderator", Password = _hashPassword.Hash("123123"),
            Profile = new EmployeeProfile(),
            RoleId = await _context.EmployeeRoles.Where(x=>x.Role=="Moderator").Select(x => x.Id).FirstAsync()
        };
        Employee owwner = new()
        {
            Nickname = "owner", Password = _hashPassword.Hash("123123"),
            Profile = new EmployeeProfile(),
            RoleId = await _context.EmployeeRoles.Where(x=>x.Role=="Owner").Select(x => x.Id).FirstAsync()
        };
        Employee newsEditor = new()
        {
            Nickname = "newsEditor",
            Password = _hashPassword.Hash("123123"),
            Profile = new NewsEditorProfile(),
            RoleId = await _context.EmployeeRoles.Where(x => x.Role == "NewsEditor").Select(x => x.Id).FirstAsync()
        };
        Employee admin = new()
        {
            Nickname = "admin", Password = _hashPassword.Hash("123123"),
            Profile = new EmployeeProfile(),
            RoleId = await _context.EmployeeRoles.Where(x=>x.Role=="Admin").Select(x => x.Id).FirstAsync()
        };
        Employee eventModerator = new()
        {
            Nickname = "EventModerator", Password = _hashPassword.Hash("123123"),
            Profile = new EmployeeProfile(),
            RoleId = await _context.EmployeeRoles.Where(x=>x.Role=="EventModerator").Select(x => x.Id).FirstAsync()
        };
        Employee organizer = new()
        {
            Nickname = "Organizer", Password = _hashPassword.Hash("123123"), Profile = new OrganizerProfile(),
            RoleId = await _context.EmployeeRoles.Where(x=>x.Role=="Organizer").Select(x => x.Id).FirstAsync()
        };
        List<Employee> employees = new() { moderator, admin, eventModerator,organizer,owwner,newsEditor };
        foreach (var employee in employees)
        {
            if (!await _context.Employees.AnyAsync(x => x.Nickname==employee.Nickname))
            {
                await _context.AddAsync(employee);
            }
        }
        await _context.SaveChangesAsync();
    }

    private async Task SeedRoles()
    {
        List<string> roles = new() { "Admin", "Moderator", "EventModerator", "Organizer", "Owner", "NewsEditor" };
        foreach (string role in roles)
        {
            if (!await _context.EmployeeRoles.AnyAsync(x => x.Role == role))
            {
                EmployeeRole employeeRole = new() { Role = role };
                await _context.AddAsync(employeeRole);
            }
        }
        await _context.SaveChangesAsync();
    }
}
