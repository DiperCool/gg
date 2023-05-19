using System.Text.Json.Serialization;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Infrastructure.Persistence;
using CleanArchitecture.WebUI.Filters;
using CleanArchitecture.WebUI.JsonConverters;
using CleanArchitecture.WebUI.Services;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Converters;
using NSwag;
using NSwag.Generation.Processors.Security;

namespace CleanArchitecture.WebUI;

public static class ConfigureServices
{
    public static IServiceCollection AddWebUIServices(this IServiceCollection services)
    {


        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddSingleton<ICurrentUserService, CurrentUserService>();

        services.AddHttpContextAccessor();

        services.AddHealthChecks()
            .AddDbContextCheck<ApplicationDbContext>();

        services.AddControllersWithViews(options =>
            {
                options.Filters.Add<ApiExceptionFilterAttribute>();
                options.ModelValidatorProviders.Clear();
            })
            .AddFluentValidation(x => x.AutomaticValidationEnabled = false);

        services.AddRazorPages().AddNewtonsoftJson(opts=>
        {
            opts.SerializerSettings.Converters.Add(new GuidConverter());
            opts.SerializerSettings.Converters.Add(new StringEnumConverter());
        });

        // Customise default API behaviour
        services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);

        services.AddOpenApiDocument(configure =>
        {
            configure.Title = "Good Games Api";

            configure.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
            {
                Type = OpenApiSecuritySchemeType.ApiKey,
                Name = "Authorization",
                In = OpenApiSecurityApiKeyLocation.Header,
                Description = "Type into the textbox: Bearer {your JWT token}."
            });
            
            configure.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));

        });
        return services;
    }
}



