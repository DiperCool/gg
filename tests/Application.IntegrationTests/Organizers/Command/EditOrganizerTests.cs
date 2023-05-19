using CleanArchitecture.Application.Organizers.Commands.EditProfileOrganizer;
using CleanArchitecture.Domain.Entities.EmployeeProfiles;
using FluentAssertions;
using NUnit.Framework;

namespace CleanArchitecture.Application.IntegrationTests.Organizers.Command;
using static Testing;
public class EditOrganizerTests:BaseTestFixture
{
    [Test]
    public async Task ShouldDenyAnonymousUser()
    {
        var command = new EditProfileOrganizerCommand();

        var action = () => SendAsync(command);
        await action.Should().ThrowAsync<UnauthorizedAccessException>();
    }
    [Test]
    public async Task ShouldEditProfile()
    {
        string id=await RunAsDefaultEmployeeAsync("Organizer");
        OrganizerProfile emp = await FindAsync<OrganizerProfile>(x => x.EmployeeId == Guid.Parse(id));
        var command = new EditProfileOrganizerCommand()
        {
            Name = "test0124444"
        };
        await SendAsync(command);
        OrganizerProfile emp2 = await FindAsync<OrganizerProfile>(x => x.EmployeeId == Guid.Parse(id));
        emp.Name.Should().NotBe(emp2.Name);

    }
}