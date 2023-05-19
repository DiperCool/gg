using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.Common.Interfaces;
public interface IEmailSender
{
    Task Send(string? content, string? subject,string? to);
    Task SendEmailConfirmation(Guid userId,string email);
    string? GetMail();
}
