using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace GamesLibrary.Application.Extentions
{
    public static class IServiceCollectionExtentions
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
        {
            services.AddMediator();
            return services;
        }

        private static void AddMediator(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        }
    }
}
