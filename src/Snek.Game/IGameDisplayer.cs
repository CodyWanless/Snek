namespace Snek.Game
{
    public interface IGameDisplayer
    {
        Task InitializeDisplayAsync(GameBoard gameBoard);

        Task DrawGameAsync(GameBoard gameBoard);

        Task DisplayLostAsync(string message);
    }
}
