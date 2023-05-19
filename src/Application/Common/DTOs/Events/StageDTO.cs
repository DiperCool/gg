using CleanArchitecture.Domain.Entities.Events;
using CleanArchitecture.Domain.Enums;

namespace CleanArchitecture.Application.Common.DTOs.Events;

public class StageDTO : IMapFrom<Stage>
{
    public string Id { get; set; } = String.Empty;
    public string EventId { get; set; } = String.Empty;
    public string Name { get; set; } = String.Empty;
    public DateTime StageStart { get; set; }
    public List<WinnerDTO> Winners { get; set; } = new();
    public List<GroupDTO> Groups { get; set; } = new();
    public View View { get; set; }
}