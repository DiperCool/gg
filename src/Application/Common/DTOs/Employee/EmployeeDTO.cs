using CleanArchitecture.Application.Common.DTOs.EmployeeProfiles;
using Profile = AutoMapper.Profile;

namespace CleanArchitecture.Application.Common.DTOs.Employee;

public class EmployeeDTO : IMapFrom<Domain.Entities.Employee>
{
    public Guid Id { get; set; }
    public string Nickname { get; set; } = String.Empty;
    public bool IsDeleted { get; set; }
    public string Role { get; set; } = null!;
    public EmployeeProfileDTO Profile { get; set; } = null!;
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Domain.Entities.Employee,EmployeeDTO>()
            .ForMember(x=>x.Role, opt=>opt.MapFrom(x=>x.Role.Role));
    }
}