using Microsoft.AspNetCore.SignalR;

namespace Homestead.Server.SignalR;

public class CommunicationHub : Hub
{
    private IGameEngine engine;

    public CommunicationHub(IGameEngine engine)
    {
        this.engine = engine;
    }

    public async Task RecieveAction(Action action)
    {
        var newState = engine.PerformAction(action);
        
        await Clients.Group(newState.GameId).SendAsync("ActionRecievedAndEvaluated", newState);
    }
}
