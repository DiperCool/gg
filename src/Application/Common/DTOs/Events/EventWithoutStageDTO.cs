using AutoMapper;
using CleanArchitecture.Application.Common.DTOs.Employee;
using CleanArchitecture.Domain.Entities.Events;
using CleanArchitecture.Domain.Enums;

namespace CleanArchitecture.Application.Common.DTOs.Events;

public class EventWithoutStageDTO : IMapFrom<Event>
{
    public string Id { get; set; } = String.Empty;
    public string Title { get; set; } = String.Empty;
    public bool IsApproved { get; set; }
    public MediaFileDTO? Picture { get; set; }
    public DateTime EventStart { get; set; }
    public DateTime EventEnd { get; set; }
    public int TopWinners { get; set; }

    public DateTime RegistrationStart { get; set; }

    public DateTime RegistrationEnd{ get; set; }
    public Regime Regime { get; set; }
    public View View { get; set; }
    public int EntryPrice { get; set; }
    public PublicEmployeeDTO Organizer { get; set; } = null!;
    public EventType EventType { get; set; }
    public bool IsPaid { get ; set; }
    public bool IsQuantityLimited { get; set; }
    public string Requirements { get; set; } = String.Empty;
    public List<EventParticipantDTO> Participants { get; set; } = new();

    public int MaxQuantity { get; set; }
    public int CurrentQuantity { get; set; }
    public WinnerDTO? Winner { get; set; }
    public PrizeDTO Prize { get; set; } = null!;
    public string Description { get; set; } = string.Empty;
}