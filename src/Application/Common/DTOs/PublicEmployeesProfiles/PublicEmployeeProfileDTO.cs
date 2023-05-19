using AutoMapper;
using CleanArchitecture.Application.Common.DTOs.EmployeeProfiles;
using CleanArchitecture.Domain.Entities.EmployeeProfiles;

namespace CleanArchitecture.Application.Common.DTOs.PublicEmployeesProfiles;

public class PublicEmployeeProfileDTO: IMapFrom<EmployeeProfile>
{
    public string Name { get; set; } = String.Empty;
    public void Mapping(Profile profile)
    {
        profile.CreateMap<OrganizerProfile, PublicOrganizerProfileDTO>();
        profile.CreateMap<NewsEditorProfile, PublicNewsEditorProfileDTO>();
        profile.CreateMap<EmployeeProfile, PublicEmployeeProfileDTO>()
            .Include<NewsEditorProfile, PublicNewsEditorProfileDTO>()
            .Include<OrganizerProfile, PublicOrganizerProfileDTO>();
    }
}