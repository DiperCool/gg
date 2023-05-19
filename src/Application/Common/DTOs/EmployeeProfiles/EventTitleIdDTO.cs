using CleanArchitecture.Domain.Entities.Events;

namespace CleanArchitecture.Application.Common.DTOs.EmployeeProfiles;

public class EventTitleIdDTO: IMapFrom<Event>
{
    public string Id { get; set; } = String.Empty;
    public string Title { get; set; } = String.Empty;
}