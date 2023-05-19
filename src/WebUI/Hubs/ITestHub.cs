using System.CodeDom.Compiler;
using SignalRSwaggerGen.Attributes;

namespace CleanArchitecture.WebUI.Hubs;
public interface ITestHub
{
    Task SendTest(string message);
}