namespace CleanArchitecture.Domain.Entities.EmployeeProfiles;

public class NewsEditorProfile : EmployeeProfile
{
    public List<News> CreatedNews { get; set; } = new();

}