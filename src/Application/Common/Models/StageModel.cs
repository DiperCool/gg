using CleanArchitecture.Domain.Entities.Events;
using CleanArchitecture.Domain.Enums;

namespace CleanArchitecture.Application.Common.Models;

public class StageModel
{
    public string Name { get; set; } = String.Empty;
    public DateTime StageStart { get; set; }
    public List<GroupModel> Groups { get; set; } = new();
    public View View { get; set; }
}