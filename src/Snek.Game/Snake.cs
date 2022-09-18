namespace Snek.Game
{
    internal class Snake
    {
        private readonly int rowCount;
        private readonly int columnCount;
        private readonly ISnakeMoveState snakeMoveState;

        public Snake(
            int rowCount,
            int columnCount)
        {
            // TODO: This is a concern of the board ,no?
            this.rowCount = rowCount;
            this.columnCount = columnCount;
            this.snakeMoveState = new SnakeMoveState();

            this.HeadNode = new SnakeNode { X = 0, Y = 0 };
            this.TailNode = this.HeadNode;
            this.Length = 1;
        }

        internal SnakeNode HeadNode { get; }

        internal SnakeNode TailNode { get; private set; }

        public int Length { get; private set; }

        public Coordinate HeadPosition => new(this.HeadNode.X, this.HeadNode.Y);

        internal void Move()
        {
            var currentNode = this.HeadNode;
            var (nextX, nextY) = this.snakeMoveState.GetNextHeadPosition(currentNode, this.rowCount, this.columnCount);

            do
            {
                var currentX = currentNode.X;
                var currentY = currentNode.Y;

                currentNode.X = nextX;
                currentNode.Y = nextY;

                nextX = currentX;
                nextY = currentY;
                currentNode = currentNode.Next;
            }
            while (currentNode != null);
        }

        internal void UpdateDirection(ConsoleKey key)
        {
            var updatedDirection = key switch
            {
                ConsoleKey.UpArrow or ConsoleKey.W => MoveDirection.Up,
                ConsoleKey.DownArrow or ConsoleKey.S => MoveDirection.Down,
                ConsoleKey.LeftArrow or ConsoleKey.A => MoveDirection.Left,
                ConsoleKey.RightArrow or ConsoleKey.D => MoveDirection.Right,
                _ => MoveDirection.None
            };

            if (updatedDirection != MoveDirection.None)
            {
                this.snakeMoveState.UpdateDirection(updatedDirection);
            }
        }

        internal void AddTail()
        {
            var currentTail = this.TailNode;
            this.TailNode = new SnakeNode
            {
                X = currentTail.X - 1,
                Y = currentTail.Y - 1,
                Next = null,
                Previous = currentTail,
            };
            currentTail.Next = this.TailNode;

            this.Length++;
        }

        internal record SnakeNode
        {
            public int X { get; internal set; }
            public int Y { get; internal set; }

            public SnakeNode? Previous { get; init; }
            public SnakeNode? Next { get; internal set; }
        }
    }
}
