using API.Application.Services;
using API.Application.Interfaces;
using API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using API.Application.DTOs;
using Microsoft.EntityFrameworkCore.Diagnostics;
namespace API.Middleware;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Add DbContext
       services.AddDbContext<DataContext>(options =>
        {
            options.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
            options.ConfigureWarnings(w => 
                w.Ignore(RelationalEventId.PendingModelChangesWarning));
        });


        // Configure JWT settings
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
        
        // Service registrations
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IMeetingCategoryService, MeetingCategoryService>();
        services.AddScoped<IMeetingSettingsService, MeetingSettingsService>();
        services.AddScoped<IMeetingTypeService, MeetingTypeService>();
        services.AddScoped<IMeetingLevelService, MeetingLevelService>();
        services.AddScoped<IAgendaItemTypeService, AgendaItemTypeService>();


        // Register RequestContext for accessing request-specific data
        services.AddHttpContextAccessor();
        services.AddScoped<IRequestContext, RequestContext>();

        return services;
    }
}
