using Homestead.Shared;
using System.Collections.Concurrent;

namespace Homestead.Server.SignalR
{
    public class GameLookup : IGameLookup
    {
        ConcurrentDictionary<string, Game> _gameLookup = new();
        public List<string> ListGames { get => _gameLookup.Keys.ToList(); }

        public Game? GetGame(string gameId)
        {
            _gameLookup.TryGetValue(gameId, out var game);
            return game;
        } 


    }
}
