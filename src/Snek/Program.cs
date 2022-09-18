using Snek;
using Snek.Game;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();

        services.AddSnakeGameWithDisplay<ConsoleGameDisplayer>();
    })
    .Build();

await host.RunAsync();
