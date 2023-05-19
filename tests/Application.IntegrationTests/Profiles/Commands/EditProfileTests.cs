using CleanArchitecture.Application.Profiles.Command.EditProfile;
using FluentAssertions;
using NUnit.Framework;

namespace CleanArchitecture.Application.IntegrationTests.Profiles.Commands;
using static Testing;
public class EditProfileTests : BaseTestFixture
{
    [Test]
    public async Task ShouldDenyAnonymousUser()
    {
        var command = new EditProfileCommand();

        var action = () => SendAsync(command);
        await action.Should().ThrowAsync<UnauthorizedAccessException>();
    }
    [Test]
    public async Task ShouldEditProfile()
    {
        string email = "test123@gmail.com"; 
        await RunAsUserAsync(email, "1234567");
        var command = new EditProfileCommand()
        {
            Email = "test1234@gmail.com"
        };

        await SendAsync(command);
        Profile profile = await FindAsync<Profile>(x => x.UserId == Guid.Parse(GetCurrentUserId()!));
        profile.Email.Should().NotBe(email);

    }
}