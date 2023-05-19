namespace CleanArchitecture.WebUI.Models;

public class CreateTeamModel
{
    public string Title { get; set; } = String.Empty;
    public string Tag { get; set; } = String.Empty;
    public IFormFile? Picture { get; set; } = null!;
}