using AutoMapper;
using CleanArchitecture.Domain.Entities.Events;
using CleanArchitecture.Domain.Enums;

namespace CleanArchitecture.Application.Common.DTOs.Events;

public class GroupDTO : IMapFrom<Group>
{
    public string Id { get; set; } = String.Empty;
    public string Name { get; set; } = String.Empty;
    public DateTime ConfirmationTimeEnd { get; set; }
    public DateTime ConfirmationTimeStart { get; set; }
    public DateTime GroupStart { get; set; }
    public Map Map { get; set; }
    public List<ParticipantDTO> AllParticipants { get; set; } = new();

    public List<ParticipantDTO> ReserveParticipants { get; set; } = new();
    public List<ParticipantDTO> Participants { get; set; } = new();
    public List<ParticipantDTO> PaidParticipants { get; set; } = new();

    public List<PlayerStatsDTO> Results { get; set; } = new();
    public List<GroupModeratorDTO> GroupModerators { get; set; } = new();

    public DateTime ReserveConfirmationTimeEnd { get; set; }

    public int ReserveSlotsQuantity { get; set; }
    public int PaidSlots { get; set; }
    public int SlotPrice { get; set; }
    public int SlotsQuantity { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Group, GroupDTO>()
            .ForMember(x => x.AllParticipants, opt => opt.MapFrom(x => x.Participants));
    }
}