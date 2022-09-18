namespace Snek.Game
{
    internal class SnakeMoveState : ISnakeMoveState
    {
        private ISnakeMoveStateStrategy moveStrategy;

        public SnakeMoveState()
        {
            this.moveStrategy = SnakeMoveStateFactory.GetStrategy(MoveDirection.Right);
        }

        public MoveDirection Direction => this.moveStrategy.Direction;

        public Coordinate GetNextHeadPosition(
            Snake.SnakeNode head,
            int rowCount,
            int columnCount) =>
            this.moveStrategy.GetNextHeadPosition(head, rowCount, columnCount);

        public void UpdateDirection(MoveDirection moveDirection)
        {
            this.moveStrategy = SnakeMoveStateFactory.GetStrategy(moveDirection);
        }

        private static class SnakeMoveStateFactory
        {
            private static readonly IReadOnlyDictionary<MoveDirection, ISnakeMoveStateStrategy> MoveStrategies =
                new Dictionary<MoveDirection, ISnakeMoveStateStrategy>
                {
                    { MoveDirection.Left, new LeftMoveStrategy() },
                    { MoveDirection.Right, new RightMoveStrategy() },
                    { MoveDirection.Up, new UpMoveStrategy() },
                    { MoveDirection.Down, new DownMoveStrategy() },
                };

            public static ISnakeMoveStateStrategy GetStrategy(MoveDirection moveDirection) =>
                MoveStrategies[moveDirection];
        }
    }
}
