using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.Common.DTOs;

public class TeamDTO: IMapFrom<Team>
{
    public Guid Id { get; set; }
    public string Title { get; set; } = String.Empty;
    public string Tag { get; set; } = String.Empty;
    public DateTime CreatedAt { get; set; }
    public MediaFileDTO Logo { get; set; } = null!;
}