﻿using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace CleanArchitecture.Application.IntegrationTests;

using static Testing;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(configurationBuilder =>
        {
            var integrationConfig = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            configurationBuilder.AddConfiguration(integrationConfig);
        });

        builder.ConfigureServices((builder, services) =>
        {
            services
                .Remove<ICurrentUserService>()
                .AddTransient(provider => Mock.Of<ICurrentUserService>(s =>
                    s.UserId == GetCurrentUserId()&&s.UserIdGuid== new Guid(GetCurrentUserId()??"00000000-0000-0000-0000-000000000000")));
            services
                .Remove<IEmailSender>()
                .AddTransient(provider=>{
                    var emailSender = new Mock<IEmailSender>();
                    emailSender.Setup(x => x.Send(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));
                    emailSender.Setup(x => x.GetMail()).Returns("");
                    emailSender.Setup(x => x.SendEmailConfirmation(It.IsAny<Guid>(),It.IsAny<string>()));
                    return emailSender.Object; 
                });
            services
                .Remove<DbContextOptions<ApplicationDbContext>>()
                .AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
        });
    }
}
