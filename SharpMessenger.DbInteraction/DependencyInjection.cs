using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using SharpMessenger.DbInteraction.Database;
using SharpMessenger.DbInteraction.Repositories.Contracts;
using SharpMessenger.DbInteraction.Repositories;

namespace SharpMessenger.DbInteraction
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInteractionWithListDB(this IServiceCollection services)
        {
            services.AddSingleton<IDbContext, ListDbContext>();
            return services;
        }

        public static IServiceCollection AddInteractionWithApplicationDatabase(this IServiceCollection services, IConfiguration configuratio) 
        {
            // todo: connectionstring, configure properly
            return null!;
        }

        public static IServiceCollection AddUserRepository(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            return services;
        }
    }
}
