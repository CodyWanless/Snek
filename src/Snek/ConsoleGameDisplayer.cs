using Snek.Game;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace Snek
{
    internal class ConsoleGameDisplayer : BaseGameDisplayer
    {
        private (int Left, int Top) origin;

        /// <inheritdoc />
        public override Task DisplayLostAsync(string message)
        {
            Console.SetCursorPosition(0, Console.BufferHeight - 1);
            Console.CursorVisible = true;

            Console.WriteLine(message);

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        [SupportedOSPlatformGuard("windows")]
        protected override Task InitializeAsync(
            int rowCount,
            int columnCount)
        {
            this.origin = Console.GetCursorPosition();

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var requiredHeight = rowCount + this.origin.Top + 2;
                var requiredWidth = columnCount + this.origin.Left + 2;

                Console.BufferHeight = Math.Max(requiredHeight, Console.BufferHeight);
                Console.BufferWidth = Math.Max(requiredWidth, Console.BufferWidth);
            }

            Console.CursorVisible = false;
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        protected override Task DrawCellAsync(
            int rowNum,
            int colNum,
            CellDisplayType cellType)
        {
            var currentColor = Console.ForegroundColor;

            var (character, color) = cellType switch
            {
                CellDisplayType.Empty => ('*', ConsoleColor.White),

                CellDisplayType.SnakeHead => ('h', ConsoleColor.Green),
                CellDisplayType.SnakeTail => ('t', ConsoleColor.Green),
                CellDisplayType.SnakeBody => ('=', ConsoleColor.Green),
                CellDisplayType.Fruit => ('f', ConsoleColor.Magenta),

                CellDisplayType.Border => ('-', ConsoleColor.Cyan),
                CellDisplayType.SideBorder => ('|', ConsoleColor.Cyan),

                _ => throw new InvalidOperationException($"Unexpected cell type {cellType})"),
            };

            var actualRowNum = this.origin!.Top + rowNum;
            var actualColNum = this.origin.Left + colNum;
            Console.SetCursorPosition(actualColNum, actualRowNum);
            Console.ForegroundColor = color;
            Console.Write(character);

            // Reset output
            Console.ForegroundColor = currentColor;

            return Task.CompletedTask;
        }
    }
}
