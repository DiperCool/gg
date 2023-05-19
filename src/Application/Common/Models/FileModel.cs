namespace CleanArchitecture.Application.Common.Models;

public class FileModel
{
    public string NameFile { get; set; } = String.Empty; 
    public byte[] Bytes  { get; set; } = new byte[0];
    public long Length { get; set; }
}
