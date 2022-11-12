using Homestead.Shared;

namespace Homestead.Client.ViewModels
{
    /// <summary>
    /// This class allows us to organize the players on the board so that the local player is on the bottom always
    /// </summary>
    public class BoardVm
    {
        public string GameId { get; internal set; }
        public Game.GameState State { get; internal set; }
        public PlayerVm LocalPlayer { get; } = null!;
        public List<PlayerVm> OtherPlayers { get; } = new();
        public int LocalPlayerNumber { get; }

        // Information about how to render the board.
        public bool LocalPlayersTurn { get; internal set; }
        public bool CanDrawFromDeck { get; internal set; } = false;
        public bool CanDrawFromDiscard { get; internal set; } = false;
        public bool CanDiscard { get; internal set; } = false;
        public bool CanEndTurn { get; internal set; } = false;
        public CardVm? TopDiscardCard { get; internal set; } = null;
        public int DiscardCardCount { get; internal set; } = 0;

        public class ActionEventArgs : EventArgs
        {
            public PlayerAction Action { get; }

            public ActionEventArgs(PlayerAction action)
            {
                Action = action;
            }
        }
        public event EventHandler<ActionEventArgs> PerformAction = null!;

        protected virtual void OnPerformAction(PlayerAction action)
        {
            EventHandler<ActionEventArgs> handler = PerformAction;
            var args = new ActionEventArgs(action);
            handler(this, args);
        }

        public void Play(PlayerCardVm card)
        {
            PlayerAction action = new(PlayerAction.ActionType.Play, LocalPlayerNumber, card.Card);
            OnPerformAction(action);
        }

        public void DrawFromDeck()
        {
            PlayerAction action = new(PlayerAction.ActionType.DrawFromDeck, LocalPlayerNumber);
            OnPerformAction(action);
        }

        public void DrawFromDiscard()
        {
            PlayerAction action = new(PlayerAction.ActionType.DrawFromDiscard, LocalPlayerNumber);
            OnPerformAction(action);
        }

        public void Discard(PlayerCardVm card)
        {
            PlayerAction action = new(PlayerAction.ActionType.Discard, LocalPlayerNumber, card.Card);
            OnPerformAction(action);
        }




        public BoardVm(Game game, int localPlayerNumber)
        {
            GameId = game.GameId;
            State = game.State;
            LocalPlayerNumber = localPlayerNumber;
            // Map the players into the A, B, C, D slot
            int otherPlayerCount = 0;
            foreach (var player in game.Players)
            {
                if (player.PlayerNumber == LocalPlayerNumber)
                {
                    LocalPlayer = new PlayerVm(player, 0);
                }
                else
                {
                    otherPlayerCount++;
                    OtherPlayers.Add(new PlayerVm(player, otherPlayerCount));
                }
            }
            Update(game);
        }

        public void Update(Game game)
        {
            State = game.State;

            // Update the players in the A, B, C, D slots
            foreach (var player in game.Players)
            {
                if (player.PlayerNumber == LocalPlayerNumber)
                {
                    LocalPlayer.Update(player);
                    LocalPlayer.IsCurrentPlayer = player.PlayerNumber == game.ActivePlayer;
                }
                else
                {
                    // Find the other player and update it
                    var otherPlayer = OtherPlayers.FirstOrDefault(f => f.PlayerNumber == player.PlayerNumber);
                    if (otherPlayer != null)
                    {
                        otherPlayer.Update(player);
                    }
                    else
                    {
                        throw new ArgumentException("Player not found");
                    }
                    otherPlayer.IsCurrentPlayer = player.PlayerNumber == game.ActivePlayer;
                }
                
            }

            // Clear options
            CanDrawFromDeck = false;
            CanDrawFromDiscard = false;
            CanEndTurn = false;
            CanDiscard = false;
            LocalPlayer.ClearPlayableCards();

            var topDiscard = game.DiscardPile.LastOrDefault();
            if (topDiscard != null)
            {
                TopDiscardCard = new CardVm(topDiscard);
            }
            else
            {
                TopDiscardCard = null;
            }
            // If it is the local player's turn then we need to update the playable cards and actions
            if (game.ActivePlayer == LocalPlayerNumber)
            {
                // Update the playable cards
                LocalPlayer.SetPlayableCards(game.AvailableActions);

                // Set other local actions
                foreach (var action in game.AvailableActions)
                {
                    if (action.Type == PlayerAction.ActionType.DrawFromDeck)
                    {
                        CanDrawFromDeck = true;
                    }
                    else if (action.Type == PlayerAction.ActionType.DrawFromDiscard)
                    {
                        CanDrawFromDiscard = true;
                    }
                    else if (action.Type == PlayerAction.ActionType.Discard)
                    {
                        CanDiscard = true;
                        // TODO: See if we want to highlight all cards as playable.
                        // This is likely to be the only available action.
                    }
                    else if (action.Type == PlayerAction.ActionType.EndTurn)
                    {
                        CanEndTurn = true;
                    }
                }
            }
            else
            {
                // Clear the playable cards
                LocalPlayer.ClearPlayableCards();
            }

        }
    }
}
