using CleanArchitecture.Domain.Entities.Events;

namespace CleanArchitecture.Domain.Entities;

public class Team
{
    public Guid Id { get; set; }
    public string Title { get; set; } = String.Empty;
    public string Tag { get; set; } = String.Empty;
    public bool IsDeleted { get; set; }
    public Guid CreatorId { get; set; }
    public User Creator { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public MediaFile Logo { get; set; } = null!;
    public string InvitationCodeLink { get; set; } = String.Empty;
    public int NumberOfMembers { get; set; }
    public string ManagerInvitationCodeLink{ get; set; } = String.Empty;
    public List<EventOnReview> OnReviews { get; set; } = new();
    public List<TeamUser> TeamUsers { get; set; } = new();
    public List<EventApproved> Approved { get; set; } = new();
    public List<Winner> EventWinners { get; set; } = new();


}