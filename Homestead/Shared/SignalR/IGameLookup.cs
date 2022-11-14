using Homestead.Shared;

namespace Homestead.Server.SignalR;
public interface IGameLookup
{
    IEnumerable<Game> ListGames { get; }

    Game? GetGame(string gameId);
    
    bool AddGame(Game game);
    void CleanupGames();
}
