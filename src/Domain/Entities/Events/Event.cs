using CleanArchitecture.Domain.Enums;

namespace CleanArchitecture.Domain.Entities.Events;

public class Event
{
    public string Id { get; set; } = String.Empty;
    public string Title { get; set; } = String.Empty;
    public bool IsApproved { get; set; }
    public MediaFile? Picture { get; set; }
    public DateTime EventStart { get; set; }
    public DateTime EventEnd { get; set; }

    public DateTime RegistrationStart { get; set; }

    public DateTime RegistrationEnd{ get; set; }
    public Regime Regime { get; set; }
    public View View { get; set; }
    public Guid OrganizerId { get; set; }
    public int EntryPrice { get; set; }
    public Employee Organizer { get; set; } = null!;
    public List<EventModerator> EventModerators { get; set; } = new();
    public List<EventParticipant> Participants { get; set; } = new();
    public EventType EventType { get; set; }
    public bool IsPaid { get ; set; }
    public int TopWinners { get; set; }
    public bool IsSponsored { get ; set; }
    public bool IsQuantityLimited { get; set; }
    public string Requirements { get; set; } = String.Empty;
    
    public List<EventOnReview> OnReviews { get; set; } = new();
    public List<EventApproved> Approved { get; set; } = new();
    public int MaxQuantity { get; set; }
    public int CurrentQuantity { get; set; }
    public Winner? Winner { get; set; }
    public Prize Prize { get; set; } = null!;
    public string Description { get; set; } = string.Empty;
    public List<Stage> Stages { get; set; } = new();

}