using CleanArchitecture.Domain.Entities.EmployeeProfiles;
using CleanArchitecture.Domain.Entities.Events;

namespace CleanArchitecture.Domain.Entities;

public class Employee
{
    public Guid Id { get; set; }
    public string Nickname { get; set; } = String.Empty;
    public string Password { get; set; } = String.Empty;
    public Guid? CreatedById { get; set; }
    public Employee? CreatedBy { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
    public Guid RoleId { get; set; }
    public EmployeeProfile Profile { get; set; } = null!;
    public List<Employee> CreatedEmployees { get; set; } = new();
    public EmployeeRole Role { get; set; } = null!;
    
    public Guid? DeletedById { get; set; }
    public Employee? DeletedBy { get; set; }
    
    public List<Employee> DeletedEmployees { get; set; } = new();
    public List<User> DeletedUsers { get; set; } = new();

    public List<Ban> Bans { get; set; } = new();
    public List<ShadowBan> ShadowBans { get; set; } = new();
    public bool IsDeleted { get; set; }
    public List<EventModerator> EventModerators { get; set; } = new();
    public List<EmployeeLog> Logs { get; set; } = new();
    public List<UserLogging> Loggings { get; set; } = new();
    public List<GroupModerator> GroupModerators { get; set; } = new();
    public List<ShopItem> CreatedShopItems { get; set; } = new();
}