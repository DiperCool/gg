using AutoMapper;
using CleanArchitecture.Application.Common.DTOs.Employee;
using CleanArchitecture.Application.Common.DTOs.PublicEmployeesProfiles;
using CleanArchitecture.Domain.Entities.EmployeeProfiles;

namespace CleanArchitecture.Application.Common.DTOs.EmployeeProfiles;

public class EmployeeProfileDTO: IMapFrom<EmployeeProfile>
{
    public string Name { get; set; } = String.Empty;
    public List<DateTime> Logging { get; set; } = new();
    public List<PublicEmployeeDTO> Created { get; set; } = new();
    public List<PublicEmployeeDTO> DeletedEmployees { get; set; } = new();
    public List<UserDTO> DeletedUsers { get; set; } = new();
    public void Mapping(Profile profile)
    {
        profile.CreateMap<OrganizerProfile, OrganizerProfileDTO>();
        profile.CreateMap<NewsEditorProfile, NewsEditorProfileDTO>()
            .ForMember(x => x.CreatedNews, opt => opt.MapFrom(x => x.CreatedNews.Select(x => x.Id)));
        profile.CreateMap<EmployeeProfile, EmployeeProfileDTO>()
            .ForMember(x=>x.Created, opt=>opt.MapFrom(x=>x.Employee.CreatedEmployees))
            .ForMember(x=>x.Logging, opt=>opt.MapFrom(x=>x.Employee.Loggings.Select(x=>x.Time)))
            
            .ForMember(x=>x.DeletedEmployees, opt=>opt.MapFrom(x=>x.Employee.DeletedEmployees))
            .ForMember(x=>x.DeletedUsers, opt=>opt.MapFrom(x=>x.Employee.DeletedUsers))
            .Include<OrganizerProfile, OrganizerProfileDTO>()
            .Include<NewsEditorProfile, NewsEditorProfileDTO>();
    }
}