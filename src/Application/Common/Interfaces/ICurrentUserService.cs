namespace CleanArchitecture.Application.Common.Interfaces;

public interface ICurrentUserService
{
    string? UserId { get; }
    Guid UserIdGuid { get; }
}
