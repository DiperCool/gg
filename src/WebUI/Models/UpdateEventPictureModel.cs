namespace CleanArchitecture.WebUI.Models;

public class UpdateEventPictureModel
{
    public string EventId { get; set; } = String.Empty;
    public IFormFile? Picture { get; set; }
}