using AutoMapper;
using CleanArchitecture.Application.Common.DTOs.Employee;
using CleanArchitecture.Domain.Entities.Events;

namespace CleanArchitecture.Application.Common.DTOs.Events;

public class GroupModeratorDTO : IMapFrom<GroupModerator>
{
    public Guid EmployeeId { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<GroupModerator, GroupModeratorDTO>()
            .ForMember(x=>x.EmployeeId, opt=>opt.MapFrom(x=>x.EmployeeId));
    }
}