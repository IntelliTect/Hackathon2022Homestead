using Homestead.Server.SignalR;
using Homestead.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Linq;

namespace Homestead.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class LobbyController
{
	private readonly CommunicationHub hub;
	private readonly IGameLookup lookup;

	public LobbyController(CommunicationHub hub, IGameLookup lookup)
	{
		this.hub = hub;
		this.lookup = lookup;
	}

	[HttpPost]
	public async Task<IActionResult> JoinGame(string lobbyId, CancellationToken token)
	{
        var game = lookup.GetGame(lobbyId);

		if (game.Players.Count > 4)
			return new BadRequestResult();

		await hub.Groups.AddToGroupAsync(hub.Context.ConnectionId, lobbyId, token);

		return new OkResult();
    }
}
