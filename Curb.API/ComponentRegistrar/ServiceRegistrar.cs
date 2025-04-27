using Curb.API.Contracts.Interfaces;
using Curb.API.DataAccess.Repositories;
using Curb.API.Services;

namespace Curb.API.ComponentRegistrar;

internal static class ServiceRegistrar
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));

        return services;
    }
}
