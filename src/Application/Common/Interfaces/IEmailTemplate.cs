namespace CleanArchitecture.Application.Common.Interfaces;

public interface IEmailTemplate
{
    string GetTemplate(string templateName);
}