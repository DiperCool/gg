using CleanArchitecture.Domain.Entities.Events;
using CleanArchitecture.Domain.Enums;

namespace CleanArchitecture.Application.Common.Models;

public class GroupModel
{
    public string Name { get; set; } = String.Empty;
    public DateTime ConfirmationTimeEnd { get; set; }
    public DateTime ConfirmationTimeStart { get; set; }
    public DateTime GroupStart { get; set; }
    public Map Map { get; set; }
    public int PaidSlots { get; set; }

    public List<GroupModeratorModel> GroupModerators { get; set; } = new();
    public string LobbyId { get; set; } = String.Empty;
    public string LobbyPassword { get; set; } = String.Empty;
    public DateTime ReserveConfirmationTimeEnd { get; set; }

    public List<ParticipantModel> Participants { get; set; } = new();
    public List<ParticipantModel> ReserveParticipants { get; set; } = new();
    public List<ParticipantModel> PaidParticipants { get; set; } = new();


    public int ReserveSlotsQuantity { get; set; }
    public int SlotPrice { get; set; }
    public int SlotsQuantity { get; set; }
}