namespace CleanArchitecture.WebUI.Models;

public class UploadPictureModel
{
    public Guid NewsId { get; set; }
    public IFormFile? Picture { get; set; } = null!;
}
