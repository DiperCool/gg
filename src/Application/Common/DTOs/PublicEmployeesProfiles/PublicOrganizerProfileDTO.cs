namespace CleanArchitecture.Application.Common.DTOs.PublicEmployeesProfiles;

public class PublicOrganizerProfileDTO : PublicEmployeeProfileDTO
{
    public MediaFileDTO? Logo { get; set; } = null!;
}