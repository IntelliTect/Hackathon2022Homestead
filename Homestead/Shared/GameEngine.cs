namespace Homestead.Shared
{
    public interface IGameEngine
    {
        Game ProcessAction(Game game, Action action);
    }

    public class GameEngine : IGameEngine
    {
        //private Game CurrentState { get; set;  }

        public Game Start(Game game)
        {
            game.ActivePlayer = 0;
            game.Actions.Add(new Action(Action.ActionType.DrawFromDeck, 0));
            return game;
        }

        public Game ProcessAction(Game game, Action action)
        {
            game.LastAction = action;
            if (action.Type is Action.ActionType.DrawFromDeck)
            {
                // Disallow if hand.count > 4, then they'll have to discard
                game.Players[game.ActivePlayer].Hand.Add(DrawCard());
            }
            else if(action.Type is Action.ActionType.Discard)
            {
                // Do not leave in!
                if (string.IsNullOrWhiteSpace(action.TargetCard)) throw new NullReferenceException();
                game.Players[game.ActivePlayer].Hand.Remove(action.TargetCard);
                game.DiscardPile.Add(action.TargetCard);
            }
            else if(action.Type is Action.ActionType.EndTurn)
            {
                // Need to revert to 0 on 3
                game.ActivePlayer++;
            }

            return game;
        }

        public string DrawCard(/*Game game*/)
        {
            // TODO: Randomly pick a card from the list of strings
            return Well;
        }

        public void DiscardCard(string card)
        {
            // Does all of these return game state?
            // Player[0].Hand.Delete(card)
        }

        public List<GameAction> PlayCard(string card)
        {
            return new List<GameAction>();
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

        private const string Well = "Well";
    }

    public class GameAction
    {

    }
}
