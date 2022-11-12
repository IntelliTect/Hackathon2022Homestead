using Homestead.Server.SignalR;
using Homestead.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Linq;
using static Homestead.Shared.Game;

namespace Homestead.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class LobbyController
{
	private readonly IHubContext<CommunicationHub> hub;
	private readonly IGameLookup lookup;

	public LobbyController(IHubContext<CommunicationHub> hub, IGameLookup lookup)
	{
		this.hub = hub;
		this.lookup = lookup;


	}

	[HttpGet("/Create")]
	public StartGameDto CreateGame([FromServices] IGameEngine engine)
	{
		var game = engine.Start();

		lookup.AddGame(game);
		return new StartGameDto { Game = game, PlayerId = 1 };
	}

	[HttpPost("/Join/{gameId}")]
	public StartGameDto Join(string gameId)
	{
        var game = lookup.GetGame(gameId);
        if (game == null) throw new ArgumentException("Game not found");
        int playerNumber;

        lock (game)
        {
            var player = game.Players.FirstOrDefault(x => x.IsBot);

            if (player is null)
            {
                throw new ArgumentException("This game is full.");
            }

            player.IsBot = false;
            playerNumber = player.PlayerNumber;
        }
		return new StartGameDto { Game = game, PlayerId = playerNumber };
	}

    [HttpGet]
	public IEnumerable<Game> GetOpenGames() 
	{
		return lookup.ListGames.Where(x => x.State == GameState.Joining);

    }
}