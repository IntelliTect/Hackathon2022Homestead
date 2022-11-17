namespace Homestead.Shared
{
    public interface IGameEngine
    {
        Game ProcessAction(Game game, PlayerAction action);
        Game Start(string playerId);
    }
}
