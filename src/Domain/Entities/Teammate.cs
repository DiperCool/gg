using CleanArchitecture.Domain.Enums;

namespace CleanArchitecture.Domain.Entities;

public class Teammate
{
    public Guid Id { get; set; }
    public TeammateType TeammateType { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}