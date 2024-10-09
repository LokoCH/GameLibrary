using GamesLibrary.Application.Repositories;
using GamesLibrary.Persistence.Contexts;
using GamesLibrary.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GamesLibrary.Persistence.Extentions
{
    public static class IServiceCollectionExtentions
    {
        public static IServiceCollection AddPersistenceLayer(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext(config);
            services.AddRepositories();
            return services;
        }

        private static void AddDbContext(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<ApplicationContext>(optionsBuilder => optionsBuilder.UseSqlServer(config.GetConnectionString("default")));
        }

        private static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IGameRepository, GameRepository>();
            services.AddScoped<IGenreRepository, GenreRepository>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        }
    }
}
