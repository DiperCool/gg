namespace CleanArchitecture.Application.Common.Security;


[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class EmployeeAuthorizeAttribute: Attribute
{
    public List<string> AllowedRoles { get; set; }

    public EmployeeAuthorizeAttribute(params string[] allowedRoles)
    {
        AllowedRoles = allowedRoles.ToList();
    }

    public EmployeeAuthorizeAttribute()
    {
        AllowedRoles = new List<string>();
    }
}