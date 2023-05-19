using System.Runtime.Intrinsics.X86;
using AutoMapper;
using CleanArchitecture.Application.Common.DTOs.Employee;
using CleanArchitecture.Domain.Entities.Events;
using CleanArchitecture.Domain.Enums;

namespace CleanArchitecture.Application.Common.DTOs.Events;

public class EventDTO: EventWithoutStageDTO, IMapFrom<Event>
{
    public List<StageDTO> Stages { get; set; } = new();
}