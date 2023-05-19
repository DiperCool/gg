using CleanArchitecture.Domain.Entities.Events;

namespace CleanArchitecture.Domain.Entities.EmployeeProfiles;

public class OrganizerProfile: EmployeeProfile
{
    public MediaFile? Logo { get; set; } = null!;
    public List<Event> CreatedEvents { get; set; } = new();
}