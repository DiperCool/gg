using CleanArchitecture.Application.Teams.Command.CreateTeam;
using FluentAssertions;
using NUnit.Framework;

namespace CleanArchitecture.Application.IntegrationTests.Teams.Commands;
using static Testing;
public class CreateTeamTests : BaseTestFixture
{
    [Test]
    public async Task ShouldDenyAnonymousUser()
    {
        var command = new CreateTeamCommand();

        var action = () => SendAsync(command);
        await action.Should().ThrowAsync<UnauthorizedAccessException>();
    }
    [Test]
    public async Task ShouldCreateTeam()
    {
        string email = "test123@gmail.com"; 
        await RunAsUserAsync(email, "1234567");
        var command = new CreateTeamCommand()
        {
            Title = "MY TEAM 123 ",
            Tag="12454",
        };

        await SendAsync(command);
        (await CountAsync<Team>()).Should().Be(1);
    }
}