namespace Homestead.Shared
{
    public interface IGameEngine
    {
        Game ProcessAction(Game game, Action action);
        Game Start();
    }
}
