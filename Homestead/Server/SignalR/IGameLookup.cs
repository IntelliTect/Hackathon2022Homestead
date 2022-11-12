using Homestead.Shared;

namespace Homestead.Server.SignalR
{
    public interface IGameLookup
    {
        Game GetGame(string gameId);
    }
}
