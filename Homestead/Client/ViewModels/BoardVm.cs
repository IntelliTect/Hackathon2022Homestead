using Homestead.Shared;

namespace Homestead.Client.ViewModels
{
    /// <summary>
    /// This class allows us to organize the players on the board so that the local player is on the bottom always
    /// </summary>
    public class BoardVm
    {
        public PlayerVm LocalPlayer { get; } = null!;
        public List<PlayerVm> OtherPlayers { get; } = new();
        public int LocalPlayerNumber { get; }

        // Information about how to render the board.
        public bool LocalPlayersTurn { get; internal set; }
        public bool CanDrawFromDeck { get; internal set; } = false;
        public bool CanDrawFromDiscard { get; internal set; } = false;
        public bool CanDiscard { get; internal set; } = false;
        public bool CanEndTurn { get; internal set; } = false;
        public CardInfo? TopDiscardCard { get; internal set; } = null;
        public int DiscardCardCount { get; internal set; } = 0;


        public void Click(CardVm card)
        {

        }


        public BoardVm(Game game, int localPlayerNumber)
        {
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
        }

        public void Update(Game game)
        {
            // Update the players in the A, B, C, D slots
            foreach (var player in game.Players)
            {
                if (player.PlayerNumber == LocalPlayerNumber)
                {
                    LocalPlayer.Update(player);
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
                }
            }

            // Clear options
            CanDrawFromDeck = false;
            CanDrawFromDiscard = false;
            CanEndTurn = false;
            CanDiscard = false;
            LocalPlayer.ClearPlayableCards();
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
