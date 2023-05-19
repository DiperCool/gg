using CleanArchitecture.Application.Organizers.Commands.CreateOrganizer;
using FluentAssertions;
using NUnit.Framework;

namespace CleanArchitecture.Application.IntegrationTests.Organizers.Command;
using static Testing;
public class CreateOrganizerTests: BaseTestFixture
{
    [Test]
    public async Task ShouldDenyAnonymousUser()
    {
        var command = new CreateOrganizerCommand();

        var action = () => SendAsync(command);
        await action.Should().ThrowAsync<UnauthorizedAccessException>();
    }
    [Test]
    public async Task ShouldDenyNonAdmin()
    {
        await RunAsDefaultEmployeeAsync("Organizer");
        var command = new CreateOrganizerCommand();

        var action = () => SendAsync(command);
        await action.Should().ThrowAsync<ForbiddenAccessException>();
    }
    [Test]
    public async Task ShouldCreateOrganizer()
    {
        await RunAsDefaultEmployeeAsync("Admin");
        var command = new CreateOrganizerCommand()
        {
            Password = "123123",
            ConfirmPassword = "123123",
            Name = "Test",
            Nickname = "test123123",
        };
        await SendAsync(command);
        (await CountAsync<Employee>(x=>x.Role.Role=="Organizer")).Should().Be(1);
    }
}