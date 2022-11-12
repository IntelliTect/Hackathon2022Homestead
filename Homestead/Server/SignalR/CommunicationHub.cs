using Homestead.Shared;
using Microsoft.AspNetCore.SignalR;

namespace Homestead.Server.SignalR;

public class CommunicationHub : Hub
{
    private readonly IGameEngine engine;
    private readonly IGameLookup gameLookup;

    public CommunicationHub(IGameEngine engine, IGameLookup gameLookup)
    {
        this.engine = engine;
        this.gameLookup = gameLookup;
    }

    public async Task ExecuteAction(string gameId, PlayerAction action)
    {
        var game = gameLookup.GetGame(gameId);
        if (game == null) throw new ArgumentNullException(nameof(game));
        if (game.State is Game.GameState.Complete)
        {
            return;
        }

        game.Players[0].IsBot = true;
        
        var newState = engine.ProcessAction(game, action);
        await Clients.Group(newState.GameId).SendAsync("ExecuteAction", newState);
        while (newState.Players[newState.ActivePlayer-1].IsBot)
        {
            if (game.State is Game.GameState.Complete)
            {
                return;
            }
            await Task.Delay(20).ConfigureAwait(false);
            Random rand = new Random();
            int index = rand.Next(0, newState.AvailableActions.Count - 1);
            var newAction = newState.AvailableActions[index];
            if (newAction.Type is PlayerAction.ActionType.Discard
                && string.IsNullOrWhiteSpace(newAction.PlayerCard))
            {
                newAction.PlayerCard = newState.Players[newState.ActivePlayer - 1].Hand.First();
            }

            newState = engine.ProcessAction(newState, newAction);
            await Clients.Group(newState.GameId).SendAsync("ExecuteAction", newState);
        }
    }
    
    public async Task RequestPushGameState(string gameId)
    {
        var game = gameLookup.GetGame(gameId);
        if (game == null) throw new ArgumentNullException(nameof(game));
        await Clients.Group(gameId).SendAsync("ActionRecieved", game);
    }
    public async Task SubscribeToGame(string gameId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, gameId);
    }
}
