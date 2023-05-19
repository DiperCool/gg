using CleanArchitecture.Application.Authenticate.Command.Register;
using CleanArchitecture.Application.Common.Exceptions;
using FluentAssertions;
using FluentValidation;
using NUnit.Framework;
using static CleanArchitecture.Application.IntegrationTests.Testing;
using ValidationException = CleanArchitecture.Application.Common.Exceptions.ValidationException;

namespace CleanArchitecture.Application.IntegrationTests.Authenticate.Commands;

public class RegisterTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireMinimumFields()
    {
        var command = new RegisterUserCommand();

        await FluentActions.Invoking(() =>
            SendAsync(command)).Should().ThrowAsync<Common.Exceptions.ValidationException>();
    }
    [Test]
    public async Task ShouldBeSamePasswordAndConfirmPassword()
    {

        var command = new RegisterUserCommand()
        {
            Email="psdiperTest234@gmail.com",
            Password="123456",
            ConfirmPassword="1234567",
            Nickname = "oleggsgsg",
            PubgId = "w55343534355"
        };
        await FluentActions.Invoking(() =>
            SendAsync(command)).Should().ThrowAsync<Common.Exceptions.ValidationException>();
    }
    [Test]
    public async Task ShouldBeUniqueEmail()
    {
        await SendAsync(new RegisterUserCommand()
        {
            Email="psdiperTest234@gmail.com",
            Name = "Test123",
            Password="123456",
            ConfirmPassword="123456",
            Nickname = "oleggsgsg",
            PubgId = "w55343534355"
            
        });

        var command = new RegisterUserCommand()
        {
            Email="psdiperTest234@gmail.com",
            Name = "Test123",
            Password="123456",
            ConfirmPassword="123456",
            Nickname = "oleggsgsg",
            PubgId = "w55343534355"
        };

        var action = () => SendAsync(command);
        await action.Should().ThrowAsync<Common.Exceptions.ValidationException>();

    }
    [Test]
    public async Task ShouldBeUniqueLogin()
    {
        await SendAsync(new RegisterUserCommand()
        {
            Email="psdiperTest234@gmail.com",
            Name = "Test123",
            Password="123456",
            ConfirmPassword="123456",
            Nickname = "oleggsgsg",
            PubgId = "w55343534355"
            
        });

        var command = new RegisterUserCommand()
        {
            Email="psdiperTest2345@gmail.com",
            Name = "Test123",
            Password="123456",
            ConfirmPassword="123456",
            Nickname = "oleggsgsg",
            PubgId = "w55343534355"
        };

        var action = () => SendAsync(command);
        await action.Should().ThrowAsync<Common.Exceptions.ValidationException>();

    }
    [Test]
    public async Task ShouldCreateUser()
    {

        var command = new RegisterUserCommand()
        {
            Email="psdiperTest234@gmail.com",
            Name = "Test123",
            Password="123456",
            ConfirmPassword="123456",
            Nickname = "oleggsgsg",
            PubgId = "w55343534355"
        };

        var result= await SendAsync(command);
        result.Should().NotBeNull();
        result.AccessToken.Should().NotBeEmpty();
    }
}
