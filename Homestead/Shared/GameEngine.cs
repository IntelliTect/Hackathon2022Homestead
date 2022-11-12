namespace Homestead.Shared
{
    public interface IGameEngine
    {
        Game ProcessAction(Game game, Action action);
    }

    public class GameEngine : IGameEngine
    {
        public Game Start()
        {
            Game game = new();
            game.GameId = Guid.NewGuid().ToString();
            const int firstPlayer = 1;

            // Create 4 players
            for (int i = firstPlayer; i <= 4; i++)
            {
                var player = new Player(i);
                if (i != firstPlayer) player.IsBot = true;
                game.Players.Add(player);
            }
            game.ActivePlayer = firstPlayer;
            game.Actions.Add(new Action(Action.ActionType.DrawFromDeck, firstPlayer));
            return game;
        }

        public Game ProcessAction(Game game, Action action)
        {
            game.LastAction = action;
            if (action.Type is Action.ActionType.DrawFromDeck)
            {
                // Disallow if hand.count > 4, then they'll have to discard
                game.Players[action.PlayerNumber].Hand.Add(Cards.GetCard());
            }
            else if(action.Type is Action.ActionType.Discard)
            {
                // Do not leave in!
                if (string.IsNullOrWhiteSpace(action.PlayerCard)) throw new NullReferenceException();
                game.Players[action.PlayerNumber].Hand.Remove(action.PlayerCard);
                game.DiscardPile.Add(action.PlayerCard);
            }
            else if(action.Type is Action.ActionType.EndTurn)
            {
                // Need to revert to 1 on 4
                game.ActivePlayer++;
            }

            return game;
        }

        public List<Action> PlayCard(string card)
        {
            return new List<Action>();
        }

        private void EvaluateActions(Game game)
        {
            
        }

        private void UpdateHomestead()
        {
            
        }

        private bool HasShelter()
        {
            // Shelter: wood, saw, hammer, stove
            return true;
        }

        private bool HasGarden()
        {
            // Garden: well, seed, shovel
            return true;
        }

        private bool HasLivestock()
        {
            // Livestock
            return true;
        }

        private Action BotChoice()
        {
            return new Action(Action.ActionType.Play, 1);
        }
    }
}
