using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Domain.Common;

namespace CleanArchitecture.WebUI.Models;

public class CreateNewsModel
{
    public string Title { get; set; }= String.Empty;
    public string Content { get; set; } =String.Empty;
    public NewsType Type { get; set; }
    public List<string> Pictures { get; set; } = new();
    public IFormFile? Background { get; set; } = null!;
}