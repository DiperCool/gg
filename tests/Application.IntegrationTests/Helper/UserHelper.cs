namespace CleanArchitecture.Application.IntegrationTests.Helper;

public static class UserHelper
{
    public static User GetUser() => new User() { Profile = new Domain.Entities.Profile()
    {
        Email = "dfdg@gmail.com", Name = "oleg", PubgId = "111111"
    }};
}