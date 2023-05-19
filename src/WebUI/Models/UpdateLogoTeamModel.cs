namespace CleanArchitecture.WebUI.Models;

public class UpdateLogoTeamModel
{
    public Guid TeamId { get; set; }
    public IFormFile? File { get; set; }
}