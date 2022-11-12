using Homestead.Server.SignalR;
using Homestead.Shared;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Homestead.Server.Controllers
{
    public class GameController : Controller
    {
        private readonly IGameLookup _lookup;
        
        public GameController(IGameLookup lookup)
        {
            _lookup = lookup;
        }

        [HttpPost("/Start/{gameId}")]
        public Game Start(string gameId)
        {
            Game? game = _lookup.GetGame(gameId);
            if (game == null) throw new ArgumentException("Game not found");
            game.State = Game.GameState.Playing;
            return game;
        }
    }
}
