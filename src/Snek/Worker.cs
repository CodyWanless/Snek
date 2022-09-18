using Snek.Game;

namespace Snek
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> logger;
        private readonly IServiceProvider serviceProvider;

        public Worker(
            ILogger<Worker> logger,
            IServiceProvider serviceProvider)
        {
            this.logger = logger;
            this.serviceProvider = serviceProvider;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
            => Task.Run(() => this.RunSnakeAsync(stoppingToken), stoppingToken);

        private async Task RunSnakeAsync(CancellationToken stoppingToken)
        {
            this.logger.LogInformation("Welcome to Snek");

            using var serviceScope = this.serviceProvider.CreateScope();
            var game = serviceScope.ServiceProvider.GetRequiredService<GameRunner>();
            await game.StartAsync(stoppingToken);
        }
    }
}