using Microsoft.AspNetCore.SignalR;

namespace Homestead.Server.SignalR;

public class ChatHub : Hub
{
    private IGameEngine engine;

    public ChatHub(IGameEngine engine)
    {
        this.engine = engine;
    }


    public async Task Draw(int number)
    {
        await Clients.All.SendAsync("RecieveDrawNotification", number);
    }

    public async Task PushGameState(IGameState state) 
    {


        await Clients.Group(state.GameId).SendAsync("RecieveGameState", state);
    }
    

    public async Task RecieveAction(string action)
    {
        var newState = engine.PerformAction(action);

        await Clients.Group(newState.GameId).SendAsync("ActionRecievedAndEvaluated", newState);
    }

}
