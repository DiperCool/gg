using System.Linq.Expressions;
using CleanArchitecture.Application.IntegrationTests.Helper;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.EmployeeProfiles;
using CleanArchitecture.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using NUnit.Framework;
using Respawn;
using Respawn.Graph;

namespace CleanArchitecture.Application.IntegrationTests;

[SetUpFixture]
public partial class Testing
{
    private static WebApplicationFactory<Program> _factory = null!;
    private static IConfiguration _configuration = null!;
    private static IServiceScopeFactory _scopeFactory = null!;
    private static IHashPassword _hashPassword = null!;
    private static Checkpoint _checkpoint = null!;
    private static string? _currentUserId;

    [OneTimeSetUp]
    public void RunBeforeAnyTests()
    {
        _factory = new CustomWebApplicationFactory();
        _scopeFactory = _factory.Services.GetRequiredService<IServiceScopeFactory>();
        _hashPassword = _factory.Services.GetRequiredService<IHashPassword>();
        _configuration = _factory.Services.GetRequiredService<IConfiguration>();

        _checkpoint = new Checkpoint
        {
            DbAdapter = DbAdapter.Postgres,
            TablesToIgnore = new Table[] { "__EFMigrationsHistory" },
        };
    }

    public static async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
    {
        using var scope = _scopeFactory.CreateScope();

        var mediator = scope.ServiceProvider.GetRequiredService<ISender>();

        return await mediator.Send(request);
    }

    public static string? GetCurrentUserId()
    {
        return _currentUserId;
    }

    public static async Task<string> RunAsDefaultUserAsync()
    {
        return await RunAsUserAsync("test@local", "Testing1234!");
    }
    public static async Task<string> RunAsDefaultEmployeeAsync(string role)
    {
        string id;
        if (role == "Organizer")
        {
            id = await CreateOrganizer("test");
        }
        else
        {
            id = await CreateAdmin("test");
        }

        _currentUserId = id;
        return id;

    }
    public static async Task CreateRoles()
    {
        using var scope = _scopeFactory.CreateScope();

        var applicationDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        List<string> roles = new() { "Admin", "Moderator", "EventModerator", "Organizer" };
        foreach (string role in roles)
        {
            if (!await applicationDbContext.EmployeeRoles.AnyAsync(x => x.Role == role))
            {
                EmployeeRole employeeRole = new() { Role = role };
                await applicationDbContext.AddAsync(employeeRole);
            }
        }
        await applicationDbContext.SaveChangesAsync();
    }

    public static async Task<string> CreateAdmin(string nickname)
    {
        await CreateRoles();
        using var scope = _scopeFactory.CreateScope();

        var applicationDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        EmployeeRole role = await FindAsync<EmployeeRole>(x => x.Role == "Admin");
        Employee admin = new()
        {
            Nickname = nickname, Password = _hashPassword.Hash("123123"),
            RoleId = role.Id
        };
        await applicationDbContext.Employees.AddAsync(admin);
        await applicationDbContext.SaveChangesAsync();
        return admin.Id.ToString();
    }
    public static async Task<string> CreateOrganizer(string nickname)
    {
        await CreateRoles();
        using var scope = _scopeFactory.CreateScope();

        var applicationDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        EmployeeRole role = await FindAsync<EmployeeRole>(x => x.Role == "Organizer");
        Employee organizer = new()
        {
            Nickname = nickname, Password = _hashPassword.Hash("123123"), Profile = new OrganizerProfile()
            {
                Name = "Organizer"
            },
            RoleId = role.Id
        };
        await applicationDbContext.Employees.AddAsync(organizer);
        await applicationDbContext.SaveChangesAsync();
        return organizer.Id.ToString();
    }
    
    
    public static async Task<string> RunAsUserAsync(string userName, string password)
    {
        using var scope = _scopeFactory.CreateScope();

        var applicationDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var user = new User {
            Profile = new()
            {
                Email = userName
            },
            Password =_hashPassword.Hash(password) };

        await applicationDbContext.Users.AddAsync(user);

       await applicationDbContext.SaveChangesAsync();

        _currentUserId = user.Id.ToString();

        return _currentUserId;
    }

    public static async Task ResetState()
    {
        using (var conn = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection")))
        {
            await conn.OpenAsync();

            await _checkpoint.Reset(conn);
        }

        _currentUserId = null;
    }

    public static async Task<User> CreateUser()
    {
        using var scope = _scopeFactory.CreateScope();

        var applicationDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var user = UserHelper.GetUser();
        await applicationDbContext.Users.AddAsync(user);

        await applicationDbContext.SaveChangesAsync();
        
        return user;
    }
    public static async Task<TEntity> FindAsync<TEntity>(Expression<Func<TEntity, bool>> predicate)
        where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        return await context.Set<TEntity>().FirstAsync(predicate);
    }

    public static async Task AddAsync<TEntity>(TEntity entity)
        where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        context.Add(entity);

        await context.SaveChangesAsync();
    }

    public static async Task<int> CountAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        return await context.Set<TEntity>().Where(predicate).CountAsync();
    }
    public static async Task<int> CountAsync<TEntity>() where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        return await context.Set<TEntity>().CountAsync();
    }

    public static string HashPassword(string password) => _hashPassword.Hash(password);
    [OneTimeTearDown]
    public void RunAfterAnyTests()
    {
    }
}
