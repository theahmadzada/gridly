using Gridly.Application.Common;
using Gridly.Application.Handlers;
using Gridly.Application.Handlers.Commands;
using Gridly.Application.ServiceContracts;
using Gridly.Application.Services;
using Gridly.Domain.Entities;
using Gridly.Infrastructure.DbContext;
using Gridly.Infrastructure.ServiceContracts;
using Gridly.Infrastructure.Services;
using Gridly.Shared.Settings;

using MediatR;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

using Serilog;

namespace Gridly.WebApi;

public static class Configuration
{
    public static IServiceCollection ConfigureMediatr(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(ConfirmEmailCommandHandler).Assembly);
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        });
        
        return services;
    }

    public static IServiceCollection ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidAudiences = configuration.GetSection("JwtSettings:Audience").Get<string[]>(),
                    ValidIssuers = configuration.GetSection("JwtSettings:Issuer").Get<string[]>(),
                    IssuerSigningKey = configuration.GetSection("JwtSettings:Key").Get<SymmetricSecurityKey>(),
                };
            });
        services.AddAuthorization();
        
        return services;
    }

    public static IServiceCollection ConfigureIdentity(this IServiceCollection services)
    {
        services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 8;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<LootDbContext>()
            .AddDefaultTokenProviders();
        
        return services;
    }

    public static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        services.AddTransient<IEmailService, EmailService>();
        services.AddTransient<IJwtService, JwtService>();
        
        return services;
    }

    public static IServiceCollection ConfigureOptions(this IServiceCollection services)
    {
        services.AddOptions<EmailSettings>()
            .BindConfiguration(EmailSettings.SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();
        services.AddOptions<JwtSettings>()
            .BindConfiguration(JwtSettings.SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();
        services.AddOptions<RefreshTokenSettings>()
            .BindConfiguration(RefreshTokenSettings.SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();
        
        return services;
    }

    public static IServiceCollection ConfigureSerilog(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSerilog((provider, lc) =>
        {
            lc.ReadFrom.Configuration(configuration);
            lc.ReadFrom.Services(provider);
        });
        
        return services;
    }

    public static IServiceCollection ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder.AllowAnyOrigin();
                builder.AllowAnyMethod();
                builder.AllowAnyHeader();
            });
        });
        
        return services;
    }
}