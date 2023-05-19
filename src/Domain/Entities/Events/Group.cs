using System.Globalization;
using CleanArchitecture.Domain.Enums;

namespace CleanArchitecture.Domain.Entities.Events;

public class Group
{
    public string Id { get; set; } = String.Empty;
    public string Name { get; set; } = String.Empty;
    public DateTime ConfirmationTimeEnd { get; set; }
    public DateTime ConfirmationTimeStart { get; set; }
    public DateTime GroupStart { get; set; }
    public Map Map { get; set; }

    public List<Participant> Participants { get; set; } = new();

    public List<PlayerStats> Results { get; set; } = new();
    public List<GroupModerator> GroupModerators { get; set; } = new();
    public string LobbyId { get; set; } = String.Empty;
    public string LobbyPassword { get; set; } = String.Empty;

    public DateTime ReserveConfirmationTimeEnd { get; set; }
    public int ReserveSlotsQuantity { get; set; }
    public int PaidSlots { get; set; }
    public int SlotPrice { get; set; }
    public int SlotsQuantity { get; set; }
    public string StageId { get; set; } = String.Empty;
    public Stage Stage { get; set; } = null!;
}