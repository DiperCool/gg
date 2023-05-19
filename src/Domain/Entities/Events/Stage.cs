using CleanArchitecture.Domain.Enums;

namespace CleanArchitecture.Domain.Entities.Events;

public class Stage
{
    public string Id { get; set; } = String.Empty;
    public Event Event { get; set; } = null!;
    public string EventId { get; set; } = String.Empty;
    public string Name { get; set; } = String.Empty;
    public DateTime StageStart { get; set; }
    public List<Winner> Winners { get; set; } = new();
    public List<Group> Groups { get; set; } = new();
    public View View { get; set; }
}