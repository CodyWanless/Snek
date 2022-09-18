using Microsoft.Extensions.DependencyInjection;

namespace Snek.Game
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSnakeGameWithDisplay<T>(
            this IServiceCollection services)
            where T : class, IGameDisplayer
        {
            services.AddTransient<GameRunner>();
            services.AddTransient<IGameDisplayer, T>();

            return services;
        }
    }
}
