namespace CleanArchitecture.Application.Common.Models
{
    public class TokenResult
    {
        public string AccessToken { get; set; } = String.Empty;
        public string RefreshToken { get; set; } = String.Empty;
    }
}