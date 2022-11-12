namespace Homestead.Shared
{
    public class GameEngine : IGameEngine
    {
        /// <summary>
        /// Creates a new game instance and returns it ready for play.<br />
        /// Assumes the active player is player 1 with the only available actions as Draw from Deck.<br />
        /// Player 1 must not be a bot.
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
            game.AvailableActions.Add(new PlayerAction(PlayerAction.ActionType.DrawFromDeck, firstPlayer));
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
        public Game ProcessAction(Game game, PlayerAction action)
        {
            game.LastActions.Add(action);
            string? playerCard = action.PlayerCard;
            List<string> playerHand = game.Players[action.PlayerNumber].Hand;
            // Probably change these to switch statements
            if (action.Type is PlayerAction.ActionType.DrawFromDeck)
            {
                // Disallow if hand.count > 4, then they'll have to discard
                // Whenever time allows, give a friendly message back to the players.
                if (game.Players[action.PlayerNumber].Hand.Count > 4) throw new OverflowException("Too much want!");
                playerHand.Add(Cards.GetCard());
            }
            else if (action.Type is PlayerAction.ActionType.DrawFromDiscard)
            {
                // Do not leave in!
                // Whenever time allows, give a friendly message back to the players.
                if (!game.DiscardPile.Any()) throw new InvalidOperationException("Sad day!");
                // Disallow if hand.count > 4, then they'll have to discard
                // Whenever time allows, give a friendly message back to the players.
                if (game.Players[action.PlayerNumber].Hand.Count > 4) throw new OverflowException("Too much want!");
                string card = game.DiscardPile.Last();
                playerHand.Add(card);
                game.DiscardPile.RemoveAt(game.DiscardPile.Count - 1);
            }
            else if (action.Type is PlayerAction.ActionType.Discard)
            {
                // Do not leave in!
                // Whenever time allows, give a friendly message back to the players.
                if (string.IsNullOrWhiteSpace(playerCard)) throw new NullReferenceException("Why did you do that?");
                // One day, we we may to check if there is more than one of this card type and to discard the chosen instance.
                // Today is not that day.
                playerHand.Remove(playerCard);
                game.DiscardPile.Add(playerCard);
            }
            // Do we throw if playerCard is null?
            else if (action.Type is PlayerAction.ActionType.Play && playerCard is not null)
            {
                if (!playerHand.Contains(playerCard)) throw new KeyNotFoundException("False!");
                // One day, we we may to check if there is more than one of this card type and to discard the chosen instance.
                // Today is not that day.
                playerHand.Remove(playerCard);
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
                else if(info.Suit is CardInfo.CardSuit.Action)
                {
                    // What happens if they draw a Good Neighbor card on the first round?
                    if(info.Name.ToUpperInvariant() is "GIVE")
                    {
                        if (string.IsNullOrWhiteSpace(action.TargetCard)) throw new NullReferenceException("Oh no!");
                        if (action.TargetPlayer is null || action.TargetPlayer is 0) throw new ArgumentException("It hurts!");
                        if (game.Players[(int)action.TargetPlayer].Hand.Count > 4) throw new ArgumentOutOfRangeException("Too much give!");

                        playerHand.Remove(action.TargetCard);
                        // Can we do this if the target player already has 5 cards?
                        game.Players[(int)action.TargetPlayer].Hand.Add(action.TargetCard);
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(action.TargetCard)) throw new NullReferenceException("!on hO");
                        if (action.TargetPlayer is null || action.TargetPlayer is 0) throw new ArgumentException("!struh tI");

                        // One day, we we may to check if there is more than one of this card type and to discard the chosen instance.
                        // Today is not that day.
                        playerHand.Remove(playerCard);
                        game.Players[(int)action.TargetPlayer].Hand.Remove(action.TargetCard);
                        playerHand.Add(action.TargetCard);
                        
                    }
                }
                // If an action goes against everyone, then that's one action per person
            }
            else if (action.Type is PlayerAction.ActionType.EndTurn)
            {
                if (game.ActivePlayer < 4)
                {
                    game.ActivePlayer++;
                }
                else
                {
                    game.ActivePlayer = 1;
                }
                // Gather list of valid actions based off of new active player.
            }

            return game;
        }

        public List<PlayerAction> PlayCard(string card)
        {
            return new List<PlayerAction>();
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

        private PlayerAction BotChoice()
        {
            return new PlayerAction(PlayerAction.ActionType.Play, 1);
        }
    }
}
