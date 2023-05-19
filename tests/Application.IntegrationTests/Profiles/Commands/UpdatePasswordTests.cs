using CleanArchitecture.Application.Profiles.Command.UpdatePassword;
using FluentAssertions;
using NUnit.Framework;

namespace CleanArchitecture.Application.IntegrationTests.Profiles.Commands;
using static Testing;
public class UpdatePasswordTests : BaseTestFixture
{
    [Test]
    public async Task ShouldDenyAnonymousUser()
    {
        var command = new UpdatePasswordCommand();

        var action = () => SendAsync(command);
        await action.Should().ThrowAsync<UnauthorizedAccessException>();
    }
    [Test]
    public async Task ShouldRequireMinimumFields()
    {
        await RunAsUserAsync("ross", "123456");
        var command = new UpdatePasswordCommand();

        await FluentActions.Invoking(() =>
            SendAsync(command)).Should().ThrowAsync<Common.Exceptions.ValidationException>();
    }
    [Test]
    public async Task ShouldUpdatePassword()
    {
        string password = "123456";
        string newPassword = "124567";
        await RunAsUserAsync("ross", password);
        var command = new UpdatePasswordCommand()
        {
            CurrentPassword = password,
            NewPassword = newPassword,
            ConfirmNewPassword =newPassword
        };
        await SendAsync(command);
        User user = await FindAsync<User>(x => x.Id == Guid.Parse(GetCurrentUserId()!));
        user.Password.Should().Be(HashPassword(newPassword));
    }
    [Test]
    public async Task ShouldMatchPassword()
    {
        string password = "123456";
        string newPassword = "124567";
        await RunAsUserAsync("ross", password);
        var command = new UpdatePasswordCommand()
        {
            CurrentPassword = password+"1",
            NewPassword = newPassword,
            ConfirmNewPassword =newPassword
        };
        var action = ()=>SendAsync(command);
        await action.Should().ThrowAsync<BadRequestException>();
    }
}