using CleanArchitecture.Domain.Entities.EmployeeProfiles;
using CleanArchitecture.Domain.Entities.Events;

namespace CleanArchitecture.Domain.Entities;

public class MediaFile
{
    public Guid Id { get; set; }
    public string FullPath { get; set; } = String.Empty;
    public string ShortPath { get; set; } = String.Empty;
    public string FileExtension { get; set; } = String.Empty;
    public long Length { get; set; }
    public Guid? ProfileId { get; set; }
    public Profile? Profile { get; set; }
    
    public Guid? OrganizerProfileId { get; set; }
    public OrganizerProfile? OrganizerProfile { get; set; }
    
    public Guid? TeamId { get; set; }
    public Team? Team { get; set; }
    
    public Guid? NewsId { get; set; }
    public News? News { get; set; }
    
    
    public Guid? BackgroundNewsId { get; set; }
    public News? BackgroundNews { get; set; }
    
    public string? EventId { get; set; }
    public Event? Event { get; set; }
}