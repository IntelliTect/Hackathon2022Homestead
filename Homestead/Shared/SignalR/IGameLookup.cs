using Homestead.Shared;

namespace Homestead.Server.SignalR;
public interface IGameLookup
{
    IEnumerable<string> ListGames { get; }

    Game? GetGame(string gameId);

    public bool AddGame(Game game);
}
