using CleanArchitecture.Application.Common.Models;

namespace CleanArchitecture.WebUI.ExtensionsMethods;

public static class ConvertatorFormFile
{
    public static async Task<FileModel> ConvertToFileModelAsync(this IFormFile? file)
    {
        var memoryStream = new MemoryStream();

        await file.CopyToAsync(memoryStream);
        FileModel fileModel = new(){ NameFile = file.FileName, Bytes= memoryStream.ToArray(), Length = file.Length };
        return fileModel;
    }
}
