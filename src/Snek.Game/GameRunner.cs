using Microsoft.Extensions.Logging;

namespace Snek.Game
{
    public sealed class GameRunner
    {
        private readonly ILogger<GameRunner> logger;
        private readonly IGameDisplayer displayer;
        private readonly Snake snake;
        private readonly GameBoard board;
        private readonly object updateLock;

        public GameRunner(
            ILogger<GameRunner> logger,
            IGameDisplayer displayer)
        {
            this.logger = logger;
            this.displayer = displayer;

            this.board = new(15, 30);
            this.snake = new(this.board.RowCount, this.board.ColumnCount);
            this.updateLock = new();
        }

        public async Task StartAsync(CancellationToken token)
        {
            this.logger.LogInformation("Welcome to Snek.");

            // Set initial state
            this.board.AddFruit();
            this.board.Update(this.snake);
            await this.displayer.InitializeDisplayAsync(this.board);

            var gameLoopTask = this.RunGameLoopAsync(token);
            var userInputTask = Task.Run(() => this.ReadUserInput(token), token);

            await Task.WhenAny(gameLoopTask, userInputTask);
        }

        private async Task RunGameLoopAsync(CancellationToken token)
        {
            var delay = TimeSpan.FromMilliseconds(500);
            do
            {
                await Task.Delay(delay, token);

                lock (this.updateLock)
                {
                    this.snake.Move();
                    this.board.Update(this.snake);
                }

                await this.displayer.DrawGameAsync(this.board);

                var gameState = GameValidator.GetTurnStatus(this.board);
                switch (gameState)
                {
                    case ValidationStatus.Nop:
                        break;
                    case ValidationStatus.GameOver:
                        await this.displayer.DisplayLostAsync($"You lose! Score: {this.snake.Length}");
                        return;
                    case ValidationStatus.DropFruit:
                        lock (this.updateLock)
                        {
                            this.snake.AddTail();
                            this.board.AddFruit();

                            if (this.snake.Length % 5 == 0)
                            {
                                delay /= 1.25;
                            }
                        }

                        await this.displayer.DrawGameAsync(this.board);
                        break;
                    case ValidationStatus.Invalid:
                    default:
                        throw new InvalidOperationException($"Invalid game state: {gameState}");
                }
            }
            while (!token.IsCancellationRequested);
        }

        private void ReadUserInput(CancellationToken token)
        {
            do
            {
                var key = Console.ReadKey();
                lock (this.updateLock)
                {
                    this.snake.UpdateDirection(key.Key);
                }
            }
            while (!token.IsCancellationRequested);
        }
    }
}
