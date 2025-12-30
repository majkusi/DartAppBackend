using DartAppClean.Application.Common.Interfaces;
using DartAppClean.Domain.Constants;
using DartAppClean.Domain.IRepositories;
using DartAppClean.Domain.Services;
using DartAppClean.Infrastructure.Data;
using DartAppClean.Infrastructure.Data.Interceptors;
using DartAppClean.Infrastructure.Identity;
using DartAppClean.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static void AddInfrastructureServices(this IHostApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("DartAppCleanDb");
        Guard.Against.Null(connectionString, message: "Connection string 'DartAppCleanDb' not found.");

        builder.Services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        builder.Services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

        builder.Services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseNpgsql(connectionString);
            options.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        });

        builder.Services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        builder.Services.AddScoped<ApplicationDbContextInitialiser>();

        // REPOSITORIES
        builder.Services.AddScoped<ITeamPlayerRepository, TeamPlayerRepository>();
        builder.Services.AddScoped<IMatchRepository, MatchRepository>();
        builder.Services.AddScoped<ITeamRepository, TeamRepository>();
        builder.Services.AddScoped<IRoundRepository, RoundRepository>();
        builder.Services.AddScoped<IMatchReadRepository, MatchReadRepository>();

        // SIGNALR HUBS     
        builder.Services.AddScoped<IMatchStateNotificationHub, MatchStateNotificationHubService>();

        // SERVICES
        builder.Services.AddScoped<ITurnOrderService, TurnOrderService>();


        builder.Services.AddAuthentication()
            .AddBearerToken(IdentityConstants.BearerScheme);
        builder.Services.AddAuthorizationBuilder();

        builder.Services
            .AddIdentityCore<ApplicationUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddApiEndpoints();

        builder.Services.AddSingleton(TimeProvider.System);
        builder.Services.AddTransient<IIdentityService, IdentityService>();

        builder.Services.AddAuthorization(options =>
            options.AddPolicy(Policies.CanPurge, policy => policy.RequireRole(Roles.Administrator)));
    }
}
