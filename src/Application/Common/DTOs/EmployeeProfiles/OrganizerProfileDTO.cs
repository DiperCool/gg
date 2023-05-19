using CleanArchitecture.Domain.Entities.EmployeeProfiles;

namespace CleanArchitecture.Application.Common.DTOs.EmployeeProfiles;

public class OrganizerProfileDTO: EmployeeProfileDTO
{
    public MediaFileDTO? Logo { get; set; } = null!;
    public List<EventTitleIdDTO> CreatedEvents { get; set; } = new();
}