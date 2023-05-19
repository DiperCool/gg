using CleanArchitecture.Application.Authenticate.Command.GenerateRestorePassword;
using CleanArchitecture.Application.Authenticate.Command.RestorePassword;
using FluentAssertions;
using NUnit.Framework;
using static CleanArchitecture.Application.IntegrationTests.Testing;
namespace CleanArchitecture.Application.IntegrationTests.Authenticate.Commands;

public class RestorePasswordTests: BaseTestFixture
{
    [Test]
    public async Task ShouldRequireMinimumFields()
    {
        var command = new RestorePasswordCommand();

        await FluentActions.Invoking(() =>
            SendAsync(command)).Should().ThrowAsync<Common.Exceptions.ValidationException>();
    }

    [Test]
    public async Task ShouldRestorePassword()
    {
        User user = await CreateUser();
        await SendAsync(new GenerateRestorePasswordCommand() { Email = user.Profile.Email });
        RestorePassword restorePassword = await FindAsync<RestorePassword>(x=>x.UserId==user.Id);
        await SendAsync(new RestorePasswordCommand()
        {
            Code = restorePassword.Id,
            Password = "123456",
            ConfirmPassword = "123456"
        });
        restorePassword = await FindAsync<RestorePassword>(x=>x.UserId==user.Id);
        User restoredPasswordUser = await FindAsync<User>(x => x.Id == user.Id);
        restoredPasswordUser.Password.Should().NotBe(user.Password);
        restorePassword.IsRestored.Should().BeTrue();
    }
    [Test]
    public async Task ShouldThrowBadRequestExceptionAlreadyRestored()
    {
        User user = await CreateUser();
        await SendAsync(new GenerateRestorePasswordCommand() { Email = user.Profile.Email });
        RestorePassword restorePassword = await FindAsync<RestorePassword>(x=>x.UserId==user.Id);
        await SendAsync(new RestorePasswordCommand()
        {
            Code = restorePassword.Id,
            Password = "123456",
            ConfirmPassword = "123456"
        });
        var func= ()=> SendAsync(new RestorePasswordCommand()
        {
            Code = restorePassword.Id,
            Password = "123456",
            ConfirmPassword = "123456"
        });
        await func.Should().ThrowAsync<BadRequestException>();
    }
    [Test]
    public async Task ShouldThrowValidationExceptionCodeDoesntExist()
    {
        var func= ()=> SendAsync(new RestorePasswordCommand()
        {
            Code = new Guid(),
            Password = "123456",
            ConfirmPassword = "123456"
        });
        await func.Should().ThrowAsync<ValidationException>();
    }
}