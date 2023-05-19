namespace CleanArchitecture.Domain.Entities;

public class RefreshToken
{
    
    public Guid Id { get; set; }
    public string Token { get; set; } = String.Empty;
    public string JwtToken { get; set; } = String.Empty;
    public bool IsUsed { get; set; }
    public bool IsRevoked { get; set; }
    public DateTime AddedDate { get; set; }
    public DateTime ExpiryDate { get; set; }
    public Guid? UserId { get; set; }
    public User? User { get; set; }
    
    public Guid? EmployeeId { get; set; }
    
    public Employee? Employee { get; set; }
    
}