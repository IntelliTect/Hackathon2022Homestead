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

	[HttpPost("/Join")]
	public async Task<int> JoinGame(string gameId)
	{
		var game = lookup.GetGame(gameId);
		int playerNumber;

		lock (game)
		{
			var player = game.Players.FirstOrDefault(x => x.IsBot);

			if (player is null)
			{
				return 0;
			}

			player.IsBot = false;
			playerNumber = player.PlayerNumber;
		}

		await hub.Groups.AddToGroupAsync(game.GameId, gameId);
		return playerNumber;
	}

	[HttpGet("/Create")]
	public async Task<int> CreateGame([FromServices] IGameEngine engine)
	{
		var game = engine.Start();

		lookup.AddGame(game);

        await hub.Groups.AddToGroupAsync(game.GameId, game.GameId);
		return 1;
	}

	[HttpPost("/JoinWithUrl/{gameId}")]
	public async Task<int> JoinGameTest(string gameId)
	{
        var game = lookup.GetGame(gameId);
        int playerNumber;

        lock (game)
        {
            var player = game.Players.FirstOrDefault(x => x.IsBot);

            if (player is null)
            {
                return 0;
            }

            player.IsBot = false;
            playerNumber = player.PlayerNumber;
        }

        await hub.Groups.AddToGroupAsync(game.GameId, gameId);
        return playerNumber;
    }

    [HttpGet]
	public IEnumerable<Game> GetOpenGames() 
	{
		return lookup.ListGames.Where(x => x.State == GameState.Joining);

    }
}