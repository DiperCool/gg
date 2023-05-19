namespace CleanArchitecture.Domain.Entities;

public class News
{
    public Guid Id { get; set; }
    public NewsType Type { get; set; }
    public string Title { get; set; } = String.Empty;
    public string Content { get; set; } = String.Empty;
    public bool IsDeleted { get; set; }

    public Guid CreatorId { get; set; }
    public Employee Creator { get; set; } = null!;
    public MediaFile? Background { get; set; } = null!;
    public List<MediaFile> Pictures { get; set; } = new();
    public DateTime CreatedAt { get; set; }
}