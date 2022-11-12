using Homestead.Shared;

namespace Homestead.Server.SignalR
{
    public abstract class IGameLookup
    {
        public abstract Game GetGame(string gameId);
    }
}
