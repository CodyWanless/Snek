namespace Snek.Game
{
    enum ValidationStatus
    {
        Invalid = 0,
        Nop = 1,
        GameOver = 2,
        DropFruit = 3,
    }

    internal static class GameValidator
    {
        public static ValidationStatus GetTurnStatus(GameBoard gameBoard)
        {
            var snakeHeadPosition = gameBoard.SnakeHeadPosition;
            var cellState = gameBoard[snakeHeadPosition.X][snakeHeadPosition.Y];

            if (cellState.HasFlag(CellValue.SnakeBody))
            {
                // Head hit body, you're done
                return ValidationStatus.GameOver;
            }

            var fruitPosition = gameBoard.FruitPosition;
            if (fruitPosition == snakeHeadPosition)
            {
                return ValidationStatus.DropFruit;
            }

            return ValidationStatus.Nop;
        }
    }
}
