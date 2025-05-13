using Curb.API.Contracts;
using Curb.API.Contracts.Interfaces;
using Curb.API.DataAccess.Repositories;
using Curb.API.Services;

namespace Curb.API.ComponentRegistrar;

internal static class ServiceRegistrar
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddSingleton<ApiConfiguration>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ISecurityService, SecurityService>();
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        return services;
    }
}
