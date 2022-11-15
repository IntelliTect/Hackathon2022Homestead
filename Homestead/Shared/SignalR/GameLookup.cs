using Homestead.Shared;
using System.Collections.Concurrent;

namespace Homestead.Server.SignalR;
public class GameLookup : IGameLookup
{
    ConcurrentDictionary<string, Game> _gameLookup = new();
    public static TimeSpan TimeUntilGameCleanup { get; set; } = TimeSpan.FromMinutes(30);

    public IEnumerable<Game> ListGames { get => _gameLookup.Values.ToList(); }

    public Game? GetGame(string gameId)
    {
        _gameLookup.TryGetValue(gameId, out var game);
        return game;
    }

    public bool AddGame(Game game)
    {
        return _gameLookup.TryAdd(game.GameId, game);
    }

    /// <summary>
    /// Used to remove old or finished games.
    /// </summary>
    private static bool _IsLocked = false;
    private static object _Lock = new();
    public void CleanupGames()
    {
        if (!_IsLocked)
        {
            lock (_Lock)
            {
                _IsLocked = true;
                foreach (var game in _gameLookup.Where(g=>g.Value.State == Game.GameState.Complete || DateTime.UtcNow - g.Value.LastPlayDate > TimeUntilGameCleanup).ToList())
                {
                    _gameLookup.TryRemove(game);
                }
                _IsLocked = false;
            }
        }
    }
}
