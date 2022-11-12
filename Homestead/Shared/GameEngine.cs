namespace Homestead.Shared
{
    public interface IGameEngine
    {
        Game ProcessAction(Game game, Action action);
    }

    public class GameEngine : IGameEngine
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Takes an action and updates the game state of the game passed in
        /// </summary>
        /// <param name="game"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        public Game ProcessAction(Game game, Action action)
        {
            game.LastAction = action;
            string? playerCard = action.PlayerCard;
            List<string> hand = game.Players[action.PlayerNumber].Hand;
            if (action.Type is Action.ActionType.DrawFromDeck)
            {
                // Disallow if hand.count > 4, then they'll have to discard
                hand.Add(Cards.GetCard());
            }
            else if (action.Type is Action.ActionType.DrawFromDiscard)
            {
                // Disallow if hand.count > 4, then they'll have to discard
                if (!game.DiscardPile.Any()) throw new InvalidOperationException("Sad day!");
                string card = game.DiscardPile.Last();
                hand.Add(card);
                game.DiscardPile.RemoveAt(game.DiscardPile.Count - 1);
            }
            else if (action.Type is Action.ActionType.Discard)
            {
                // Do not leave in!
                if (string.IsNullOrWhiteSpace(playerCard)) throw new NullReferenceException("Why did you do that?");
                hand.Remove(playerCard);
                game.DiscardPile.Add(playerCard);
            }
            else if (action.Type is Action.ActionType.Play)
            {
                // Depending on the card we need to do different things.
                hand.Remove(playerCard);
                CardInfo info = Cards.GetCardInfo(playerCard);
                if (info.Suit is CardInfo.CardSuit.LiveStock
                    || info.Suit is CardInfo.CardSuit.Garden
                    || info.Suit is CardInfo.CardSuit.House)
                {
                    // Need to add all sorts of logic around:
                    //  Does the card already exist?
                    //  Does it form a group?
                    //  Does it result in victory?
                    game.Players[action.PlayerNumber].Board.Add(info.Card);
                }
                // If an action goes against everyone, then that's one action per person
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
