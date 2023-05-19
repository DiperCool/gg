using AutoMapper;
using CleanArchitecture.Domain.Entities.Events;

namespace CleanArchitecture.Application.Common.DTOs.Events;

public class PlacementPrizeDTO : IMapFrom<PlacementPrize>
{
    public int Number { get; set; }
    public int Prize { get; set; }
}