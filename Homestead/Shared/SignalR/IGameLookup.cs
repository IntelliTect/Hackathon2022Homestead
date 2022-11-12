using Homestead.Shared;

namespace Homestead.Server.SignalR;
public interface IGameLookup
{
    IEnumerable<Game> ListGames { get; }

    Game? GetGame(string gameId);

    public bool AddGame(Game game);
}
