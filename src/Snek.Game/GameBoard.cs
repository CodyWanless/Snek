namespace Snek.Game
{
    [Flags]
    public enum CellValue
    {
        Invalid = 0,
        SnakeHead = 1,
        SnakeBody = 2,
        Fruit = 4,
        Empty = 8,
    }

    public record Coordinate(int X, int Y);

    public class GameBoard
    {
        private readonly Random random;
        private readonly CellValue[][] cells;
        private Coordinate? snakeHeadCoordinate;
        private Coordinate? fruitCoordinate;
        private Coordinate? previousTail;

        public GameBoard(
            int gameSize)
            : this(gameSize, gameSize)
        {
            if (gameSize < 1)
            {
                throw new ArgumentException($"{nameof(gameSize)} must be larger than 0", nameof(gameSize));
            }
        }

        public GameBoard(
            int rowCount,
            int columnCount)
        {
            if (rowCount < 1)
            {
                throw new ArgumentException($"{nameof(rowCount)} must be larger than 0", nameof(rowCount));
            }

            if (columnCount < 1)
            {
                throw new ArgumentException($"{nameof(columnCount)} must be larger than 0", nameof(columnCount));
            }

            this.random = new Random();
            this.cells = new CellValue[rowCount][];
            for (var i = 0; i < rowCount; i++)
            {
                this.cells[i] = new CellValue[columnCount];
                Array.Fill(this.cells[i], CellValue.Empty);
            }
        }

        public CellValue[] this[int rowNum] => this.cells[rowNum];

        public CellValue this[int rowNum, int colNum] => this.cells[rowNum][colNum];

        public Coordinate SnakeHeadPosition => this.snakeHeadCoordinate!;

        public Coordinate FruitPosition => this.fruitCoordinate!;

        public int RowCount => this.cells.Length;

        public int ColumnCount => this.cells[0].Length;

        internal void Update(Snake snake)
        {
            if (this.previousTail != null)
            {
                this[this.previousTail.X][this.previousTail.Y] = CellValue.Empty;
            }

            // Assume snake is most recent snake
            var currentNode = snake.HeadNode;
            this[currentNode.X][currentNode.Y] = CellValue.SnakeHead;
            this.snakeHeadCoordinate = new(currentNode.X, currentNode.Y);

            Snake.SnakeNode previousNode;
            do
            {
                previousNode = currentNode;
                currentNode = currentNode.Next;
                if (currentNode != null)
                {
                    this[currentNode.X][currentNode.Y] = CellValue.SnakeBody;
                }
            }
            while (currentNode != null);

            this.previousTail = new(previousNode.X, previousNode.Y);
        }

        internal void AddFruit()
        {
            var (x, y) = this.GetOpenCoordinate();
            this[x][y] = CellValue.Fruit;
            this.fruitCoordinate = new(x, y);
        }

        private (int x, int y) GetOpenCoordinate()
        {
            // TODO: Fix performance
            while (true)
            {
                var x = this.random.Next(this.cells.Length);
                var y = this.random.Next(this.cells[0].Length);

                if (this[x][y].HasFlag(CellValue.Empty))
                {
                    return (x, y);
                }
            }
        }
    }
}
