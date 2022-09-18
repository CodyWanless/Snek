using static Snek.Game.Snake;

namespace Snek.Game
{
    internal interface ISnakeMoveStateStrategy
    {
        MoveDirection Direction { get; }

        Coordinate GetNextHeadPosition(SnakeNode head, int rowCount, int columnCount);
    }

    internal class UpMoveStrategy : ISnakeMoveStateStrategy
    {
        public MoveDirection Direction { get; } = MoveDirection.Up;

        public Coordinate GetNextHeadPosition(
            SnakeNode head,
            int rowCount,
            int columnCount)
        {
            var x = head.X;
            var y = head.Y;

            if (--x < 0)
            {
                x = rowCount - 1;
            }

            return new(x, y);
        }
    }

    internal class RightMoveStrategy : ISnakeMoveStateStrategy
    {
        public MoveDirection Direction { get; } = MoveDirection.Right;

        public Coordinate GetNextHeadPosition(
            SnakeNode head,
            int rowCount,
            int columnCount)
        {
            var x = head.X;
            var y = head.Y;

            if (++y >= columnCount)
            {
                y = 0;
            }

            return new(x, y);
        }
    }

    internal class LeftMoveStrategy : ISnakeMoveStateStrategy
    {
        public MoveDirection Direction { get; } = MoveDirection.Left;

        public Coordinate GetNextHeadPosition(
            SnakeNode head,
            int rowCount,
            int columnCount)
        {
            var x = head.X;
            var y = head.Y;

            if (--y < 0)
            {
                y = columnCount - 1;
            }

            return new(x, y);
        }
    }

    internal class DownMoveStrategy : ISnakeMoveStateStrategy
    {
        public MoveDirection Direction { get; } = MoveDirection.Down;

        public Coordinate GetNextHeadPosition(
            SnakeNode head,
            int rowCount,
            int columnCount)
        {
            var x = head.X;
            var y = head.Y;

            if (++x >= rowCount)
            {
                x = 0;
            }

            return new(x, y);
        }
    }
}
