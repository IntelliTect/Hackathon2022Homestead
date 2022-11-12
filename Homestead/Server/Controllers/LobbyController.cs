using Homestead.Server.SignalR;
using Microsoft.AspNetCore.Mvc;

namespace Homestead.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class LobbyController
{
	private readonly CommunicationHub hub;

	public LobbyController(CommunicationHub hub, IGameLookup lookup)
	{
		this.hub = hub;
		this.lookup = lookup;
	}

	[HttpPost]
	public async Task JoinGame(string lobbyId, CancellationToken token)
	{
		

		await hub.Groups.AddToGroupAsync(hub.Context.ConnectionId, lobbyId, token);

    }

}
