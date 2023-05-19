using CleanArchitecture.Application.Authenticate.Query.GetMe;

namespace CleanArchitecture.Application.IntegrationTests.Authenticate.Queries;

using CleanArchitecture.Application.Common.Security;
using FluentAssertions;
using NUnit.Framework;
using static Testing;
public class GetMeTest : BaseTestFixture
{
    [Test]
    public async Task ShouldDenyAnonymousUser()
    {
        var command = new AuthUserQuery();

        command.GetType().Should().BeDecoratedWith<AuthorizeAttribute>();

        var action = () => SendAsync(command);

        await action.Should().ThrowAsync<UnauthorizedAccessException>();
    }
    [Test]
    public async Task ShouldNotBeEmpty()
    {
        var id = await RunAsDefaultUserAsync();

        var query = new AuthUserQuery();

        var result = await SendAsync(query);

        result.Id.Should().Be(id);
    }
}
