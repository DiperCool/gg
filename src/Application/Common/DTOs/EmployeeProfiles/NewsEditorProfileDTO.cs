namespace CleanArchitecture.Application.Common.DTOs.EmployeeProfiles;

public class NewsEditorProfileDTO : EmployeeProfileDTO
{
    public List<Guid> CreatedNews { get; set; } = new();
}