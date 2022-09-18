using static Snek.Game.Snake;

namespace Snek.Game
{
    enum MoveDirection
    {
        None = 0,
        Left = 1,
        Right = 2,
        Up = 3,
        Down = 4,
    }

    internal interface ISnakeMoveState
    {
        MoveDirection Direction { get; }

        Coordinate GetNextHeadPosition(SnakeNode head, int rowCount, int columnCount);

        void UpdateDirection(MoveDirection moveDirection);
    }
}
