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
            game.LastActions.Clear();
            game.LastActions.Add(action);
            string? playerCard = action.PlayerCard;
            Player player = game.Players[action.PlayerNumber - 1];
            List<string> playerHand = player.Hand;
            // Probably change these to switch statements
            // Or investigate different ways to do this a bit cleaner.
            if (action.Type is PlayerAction.ActionType.DrawFromDeck)
            {
                // Whenever time allows, give a friendly message back to the players.
                if (playerHand.Count > 4) throw new OverflowException("Too much want!");
                var drawnCard = Cards.GetCard();
                playerHand.Add(drawnCard);

                game = EvaluateNextActions(game);

                //if (Cards.GetCardInfo(drawnCard).Suit is CardInfo.CardSuit.Disaster)
                //{
                //    game.AvailableActions.Add(new PlayerAction(PlayerAction.ActionType.Play, game.ActivePlayer, drawnCard));
                //}
                //else
                //{
                //    game = EvaluateActions(game);
                //    // To alleviate exception above,
                //    //  maybe change this to disallow ending the turn if hand.count > 4
                //    // Actually... We also need to make sure the player does not have two of the same homestead card in-hand.
                //    // Maybe now we resurrect EvaluateActions?
                //    //game.AvailableActions.Add(new PlayerAction(PlayerAction.ActionType.Play, game.ActivePlayer));
                //    //game.AvailableActions.Add(new PlayerAction(PlayerAction.ActionType.EndTurn, game.ActivePlayer));
                //    //game.AvailableActions.Add(new PlayerAction(PlayerAction.ActionType.Discard, game.ActivePlayer));
                //}
            }
            else if (action.Type is PlayerAction.ActionType.DrawFromDiscard)
            {
                // Note: we can probably consolidate this code with the above DrawFromDeck code.
                //  There's not many differences other than:
                //    Disallow someone to draw a diaster card from the discard pile.
                //    Play a disaster card right away from the deck.


                // Do not leave in!
                // Whenever time allows, give a friendly message back to the players.
                if (!game.DiscardPile.Any()) throw new InvalidOperationException("Sad day!");
                // Whenever time allows, give a friendly message back to the players.
                if (game.Players[action.PlayerNumber].Hand.Count > 4) throw new OverflowException("Too much want!");

                string card = game.DiscardPile.Last();
                // Need to disallow if last discarded card was a disaster or something destroyed by a disaster.
                // Or maybe those cards never make it to discard and they just g away.
                playerHand.Add(card);
                game.DiscardPile.RemoveAt(game.DiscardPile.Count - 1);

                game = EvaluateNextActions(game);
                //game.AvailableActions.Add(new PlayerAction(PlayerAction.ActionType.Play, game.ActivePlayer));
                //game.AvailableActions.Add(new PlayerAction(PlayerAction.ActionType.EndTurn, game.ActivePlayer));
                //game.AvailableActions.Add(new PlayerAction(PlayerAction.ActionType.Discard, game.ActivePlayer));
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

                game = EvaluateNextActions(game);
            }
            // Do we throw if playerCard is null?
            else if (action.Type is PlayerAction.ActionType.Play && playerCard is not null)
            {
                if (!playerHand.Contains(playerCard)) throw new KeyNotFoundException("False!");
                // One day, we we may to check if there is more than one of this card type and to discard the chosen instance.
                // Today is not that day.

                if (!player.Board.Contains(playerCard))
                {
                    playerHand.Remove(playerCard);
                    player.Board.Add(playerCard);
                }

                game = EvaluateNextActions(game);

                //CardInfo info = Cards.GetCardInfo(playerCard);
                //if (info.Suit is CardInfo.CardSuit.LiveStock
                //    || info.Suit is CardInfo.CardSuit.Garden
                //    || info.Suit is CardInfo.CardSuit.House)
                //{
                //    // Need to add all sorts of logic around:
                //    //  Does the card already exist?
                //    //  Does it form a group?
                //    //  Does it result in victory?
                //    game.Players[action.PlayerNumber].Board.Add(info.Card);
                //}
                //else if (info.Suit is CardInfo.CardSuit.Action)
                //{
                //    // What happens if they draw a Good Neighbor card on the first round?
                //    if (info.Name.ToUpperInvariant() is "GIVE")
                //    {
                //        if (string.IsNullOrWhiteSpace(action.TargetCard)) throw new NullReferenceException("Oh no!");
                //        if (action.TargetPlayer is null || action.TargetPlayer is 0) throw new ArgumentException("It hurts!");
                //        if (game.Players[(int)action.TargetPlayer].Hand.Count > 4) throw new ArgumentOutOfRangeException("Too much give!");

                //        playerHand.Remove(action.TargetCard);
                //        // Can we do this if the target player already has 5 cards?
                //        game.Players[(int)action.TargetPlayer].Hand.Add(action.TargetCard);
                //    }
                //    else
                //    {
                //        if (string.IsNullOrWhiteSpace(action.TargetCard)) throw new NullReferenceException("!on hO");
                //        if (action.TargetPlayer is null || action.TargetPlayer is 0) throw new ArgumentException("!struh tI");

                //        // One day, we we may to check if there is more than one of this card type and to discard the chosen instance.
                //        // Today is not that day.
                //        playerHand.Remove(playerCard);
                //        game.Players[(int)action.TargetPlayer].Hand.Remove(action.TargetCard);
                //        playerHand.Add(action.TargetCard);
                //    }
                //}
                //else if (info.Suit is CardInfo.CardSuit.Disaster)
                //{
                //    if(info.Card is Cards.WolfAll)
                //    {
                //        for(int i = 1; i < 5; i++)
                //        {
                //            game.AvailableActions.Add(
                //                new PlayerAction(
                //                    PlayerAction.ActionType.Discard,
                //                    action.PlayerNumber,
                //                    info.Card, i,
                //                    info.ImpactedCard));
                //            // Need to handle how to prevent.
                //        }
                //    }
                //}
                // If an action goes against everyone, then that's one action per person
                // game = EvaluateActions(game);
            }
            else if (action.Type is PlayerAction.ActionType.EndTurn)
            {
                game.AvailableActions.Clear();
                if (game.ActivePlayer < 4)
                {
                    game.ActivePlayer++;
                }
                else
                {
                    game.ActivePlayer = 1;
                }
                game.AvailableActions.Add(new PlayerAction(PlayerAction.ActionType.DrawFromDeck, game.ActivePlayer));
                if (game.DiscardPile.Any()
                    && Cards.GetCardInfo(game.DiscardPile.Last()).Suit is not CardInfo.CardSuit.Disaster)
                {
                    game.AvailableActions.Add(new PlayerAction(PlayerAction.ActionType.DrawFromDiscard, game.ActivePlayer));
                }
            }

            game.LastPlayDate = DateTime.UtcNow;
            return game;
        }

        public List<PlayerAction> PlayCard(string card)
        {
            return new List<PlayerAction>();
        }

        private Game EvaluateNextActions(Game game)
        {
            game.AvailableActions.Clear();

            Player player = game.Players[game.ActivePlayer - 1];
            List<string> hand = player.Hand;

            List<CardInfo> cards = new();

            // Similar to the if-elses in ProcessAction,
            //  There's got to be a better way.
            foreach (string card in hand)
            {
                cards.Add(Cards.GetCardInfo(card));
            }

            //if (game.LastActions.Any(a =>
            //    a.Type is PlayerAction.ActionType.DrawFromDeck
            //    || a.Type is PlayerAction.ActionType.DrawFromDiscard))
            //{
            //    throw new InvalidOperationException("Cannot draw");
            //    // Cannot draw
            //}

            if (player.Board.Contains(Cards.Livestock)
                && player.Board.Contains(Cards.Seeds)
                && player.Board.Contains(Cards.Well)
                && player.Board.Contains(Cards.Shovel)
                && player.Board.Contains(Cards.Saw)
                && player.Board.Contains(Cards.Hammer)
                && player.Board.Contains(Cards.Wood)
                && player.Board.Contains(Cards.Stove))
            {
                game.State = Game.GameState.Complete;
                game.Winner = game.ActivePlayer;
            }
            else
            {
                if (!hand.Any())
                {
                    game.AvailableActions.Add(new PlayerAction(PlayerAction.ActionType.EndTurn, game.ActivePlayer));
                }
                else
                {
                    var playableCards = hand.Except(player.Board);
                    if (playableCards.Any())
                    {
                        foreach (string card in playableCards)
                        {
                            game.AvailableActions.Add(new PlayerAction(PlayerAction.ActionType.Play, game.ActivePlayer, card));
                        }
                    }
                    else
                    {
                        //if (hand.Count < 5)
                        //{
                        //    game.AvailableActions.Add(new PlayerAction(PlayerAction.ActionType.EndTurn, game.ActivePlayer));
                        //}
                        if(hand.Count > 4)
                        {
                            game.AvailableActions.Add(new PlayerAction(PlayerAction.ActionType.Discard, game.ActivePlayer));
                        }
                        else
                        {
                            game.AvailableActions.Add(new PlayerAction(PlayerAction.ActionType.EndTurn, game.ActivePlayer));
                        }
                    }
                }
            }





            return game;

            //if (hand.Any())
            //{
            //    game.AvailableActions.Add(new PlayerAction(PlayerAction.ActionType.Discard, game.ActivePlayer));
            //}













            //if (hand.Count < 1)
            //{
            //    game.AvailableActions.Add(new PlayerAction(PlayerAction.ActionType.EndTurn, game.ActivePlayer));
            //}
            //else
            //{
            //    if (cards.Any(c => c.Suit is CardInfo.CardSuit.Disaster))
            //    {
            //        game.AvailableActions.Add(
            //            new PlayerAction(
            //                PlayerAction.ActionType.Play, 
            //                game.ActivePlayer, 
            //                cards.First(c => c.Suit is CardInfo.CardSuit.Disaster).Card));
            //    }
            //    else
            //    {
            //        if (hand.Count < 5
            //            && hand.Count(c => c is Cards.Livestock) < 2
            //            && hand.Count(c => c is Cards.Seeds) < 2
            //            && hand.Count(c => c is Cards.Well) < 2
            //            && hand.Count(c => c is Cards.Shovel) < 2
            //            && hand.Count(c => c is Cards.Saw) < 2
            //            && hand.Count(c => c is Cards.Hammer) < 2
            //            && hand.Count(c => c is Cards.Wood) < 2
            //            && hand.Count(c => c is Cards.Stove) < 2)
            //        {
            //            game.AvailableActions.Add(new PlayerAction(PlayerAction.ActionType.EndTurn, game.ActivePlayer));
            //        }
            //        game.AvailableActions.Add(new PlayerAction(PlayerAction.ActionType.Play, game.ActivePlayer));
            //        game.AvailableActions.Add(new PlayerAction(PlayerAction.ActionType.Discard, game.ActivePlayer));
            //    }
            //}

            //// Make sure player cannot play a homestead card if it's already on the board

            //return game;
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

        private PlayerAction BotChoice(Game game)
        {
            // Maybe switch to random just for fun until we can work on something more in depth.
            return game.AvailableActions.First();
        }
    }
}
