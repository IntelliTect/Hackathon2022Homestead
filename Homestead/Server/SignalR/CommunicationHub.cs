using Homestead.Shared;
using Microsoft.AspNetCore.SignalR;

namespace Homestead.Server.SignalR;

public class CommunicationHub : Hub
{
    private IGameEngine engine;
    private IGameLookup gameLookup;

    public CommunicationHub(IGameEngine engine, IGameLookup gameLookup)
    {
        this.engine = engine;
    }

    public async Task RecieveAction(string gameId, Shared.Action action)
    {
        var game = gameLookup.GetGame(gameId);
        var newState = engine.ProcessAction(game, action);  
        await Clients.Group(newState.GameId).SendAsync("ActionRecievedAndEvaluated", newState);
    }
}
