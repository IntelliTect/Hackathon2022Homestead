using Homestead.Server.SignalR;
using Microsoft.AspNetCore.Mvc;

namespace Homestead.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class LobbyController
{
	private readonly CommunicationHub hub;

	public LobbyController(CommunicationHub hub)
	{
		this.hub = hub;
	}

	[HttpPost]
	public async Task JoinGame(string lobbyId, CancellationToken token)
	{
		hu

		await hub.Groups.AddToGroupAsync(hub.Context.ConnectionId, lobbyId, token);

    }

}
