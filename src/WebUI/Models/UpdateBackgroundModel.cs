namespace CleanArchitecture.WebUI.Models;

public class UpdateBackgroundModel
{
    public Guid NewsId { get; set; }
    public IFormFile? File { get; set; }
}