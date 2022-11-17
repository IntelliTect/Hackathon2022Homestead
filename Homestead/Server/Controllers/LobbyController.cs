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

	[HttpPost("/Create/{playerId}")]
	public StartGameDto CreateGame([FromServices] IGameEngine engine, string playerId)
	{
		var game = engine.Start(playerId);

		lookup.AddGame(game);
		return new StartGameDto { Game = game, PlayerNumber = 1 };
	}

	[HttpPost("/Join/{gameId}/{playerId}")]
	public StartGameDto Join(string gameId, string playerId)
	{
        var game = lookup.GetGame(gameId);
        if (game == null) throw new ArgumentException("Game not found");
        int playerNumber;
		// See if the player is already a player
		var existingPlayer = game.Players.FirstOrDefault(f => f.PlayerId == playerId);
		if (existingPlayer != null)
		{
            return new StartGameDto { Game = game, PlayerNumber = existingPlayer.PlayerNumber };
        }


        lock (game)
        {
            var player = game.Players.FirstOrDefault(x => x.IsBot);

            if (player is null)
            {
                throw new ArgumentException("This game is full.");
            }

            player.IsBot = false;
            playerNumber = player.PlayerNumber;
			player.PlayerId = playerId;
        }
		return new StartGameDto { Game = game, PlayerNumber = playerNumber };
	}

    [HttpGet]
	public IEnumerable<Game> GetOpenGames() 
	{
		// Remove any games that are 30 minutes old regardless of state
		lookup.CleanupGames();
		return lookup.ListGames.Where(x => x.State != GameState.Complete);

    }
}