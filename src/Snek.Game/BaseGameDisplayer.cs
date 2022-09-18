namespace Snek.Game
{
    public enum CellDisplayType
    {
        Invalid = 0,
        SnakeHead = 1,
        SnakeBody = 2,
        SnakeTail = 4,
        Fruit = 8,
        Empty = 16,
        SideBorder = 32,
        Border = 64,
    }

    public abstract class BaseGameDisplayer : IGameDisplayer
    {
        private CellDisplayType[][]? cellDisplayValues;

        public async Task InitializeDisplayAsync(GameBoard gameBoard)
        {
            this.cellDisplayValues = new CellDisplayType[gameBoard.RowCount][];
            for (var i = 0; i < gameBoard.RowCount; i++)
            {
                this.cellDisplayValues[i] = new CellDisplayType[gameBoard.ColumnCount];
                Array.Fill(this.cellDisplayValues[i], CellDisplayType.Empty);
            }

            await this.InitializeAsync(gameBoard.RowCount, gameBoard.ColumnCount);

            for (var i = 1; i <= gameBoard.RowCount; i++)
            {
                await this.DrawCellAsync(i, 0, CellDisplayType.SideBorder);
                await this.DrawCellAsync(i, gameBoard.ColumnCount + 1, CellDisplayType.SideBorder);
            }

            for (var j = 1; j <= gameBoard.ColumnCount; j++)
            {
                await this.DrawCellAsync(0, j, CellDisplayType.Border);
                await this.DrawCellAsync(gameBoard.RowCount + 1, j, CellDisplayType.Border);
            }

            await this.DrawGameboardAsync(
                gameBoard,
                (x, y, curr) =>
                {
                    this.cellDisplayValues[x][y] = curr;
                    return true;
                });
        }

        public Task DrawGameAsync(GameBoard gameBoard)
        {
            return this.DrawGameboardAsync(
                gameBoard,
                (row, column, curr) =>
                {
                    var previousValue = this.cellDisplayValues![row][column];
                    if (curr != previousValue)
                    {
                        this.cellDisplayValues[row][column] = curr;
                        return true;
                    }

                    return false;
                });
        }

        private async Task DrawGameboardAsync(
            GameBoard gameBoard,
            Func<int, int, CellDisplayType, bool> updateCellFunc)
        {
            for (var i = 0; i < this.cellDisplayValues!.Length; i++)
            {
                for (var j = 0; j < this.cellDisplayValues[i].Length; j++)
                {
                    var cellValue = gameBoard[i][j];
                    var cellType = cellValue switch
                    {
                        CellValue.SnakeHead => CellDisplayType.SnakeHead,
                        CellValue.SnakeBody => CellDisplayType.SnakeBody,
                        CellValue.Fruit => CellDisplayType.Fruit,
                        CellValue.Invalid => CellDisplayType.Invalid,
                        CellValue.Empty => CellDisplayType.Empty,
                        _ => CellDisplayType.Invalid,
                    };

                    if (updateCellFunc(i, j, cellType))
                    {
                        await this.DrawCellAsync(i + 1, j + 1, cellType);
                    }
                }
            }
        }

        public abstract Task DisplayLostAsync(string message);

        protected abstract Task InitializeAsync(
            int rowCount,
            int columnCount);

        protected abstract Task DrawCellAsync(
            int rowNum,
            int cellNum,
            CellDisplayType cellType);
    }
}
