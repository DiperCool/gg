using Microsoft.AspNetCore.SignalR;
using SignalRSwaggerGen.Attributes;

namespace CleanArchitecture.WebUI.Hubs;
public class TestHub : Hub<ITestHub>
{
    public async Task Send(string name)
    {
        await Clients.All.SendTest(name);
    }
}