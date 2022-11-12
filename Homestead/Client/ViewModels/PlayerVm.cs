using Homestead.Shared;
using static Homestead.Shared.PlayerAction;

namespace Homestead.Client.ViewModels
{
    public class PlayerVm
    {
        public int PlayerNumber { get; }
        public Player Player { get; }
        /// <summary>
        /// The local player is always index 0
        /// </summary>
        public int PlayerIndex { get; }


        public List<CardVm> Hand
        { get; } = new();
        public List<CardVm> Board { get; } = new();


        public PlayerVm(Player player, int playerIndex)
        {
            Player = player;
            PlayerNumber = player.PlayerNumber;
            PlayerIndex = playerIndex;
        }

        public bool IsLocalPlayer => PlayerIndex == 0;

        /// <summary>
        /// This updates the viewModel when we get a new player
        /// </summary>
        /// <param name="player"></param>
        public void Update(Player player)
        {
            // Mark all cards in the hand as not updated
            Hand.ForEach(f => f.Updated = false);
            // Iterate the hand and make changes
            foreach (var card in player.Hand)
            {
                // Find the card
                var cardVm = Hand.FirstOrDefault(f => f.Card == card && !f.Updated);
                if (cardVm == null)
                {
                    // Add the card
                    Hand.Add(new CardVm(card));
                }
                else
                {
                    // Mark the card as updated
                    cardVm.Updated = true;
                }
            }
            // Remove all non-updated cards
            // TODO: This is where we could put an animation
            Hand.RemoveAll(f => !f.Updated);

            // Iterate the Board and make changes
            // Mark all cards in the hand as not updated
            Board.ForEach(f => f.Updated = false);
            // Iterate the hand and make changes
            foreach (var card in player.Board)
            {
                // Find the card
                var cardVm = Board.FirstOrDefault(f => f.Card == card && !f.Updated);
                if (cardVm == null)
                {
                    // Add the card
                    Board.Add(new CardVm(card));
                }
                else
                {
                    // Mark the card as updated
                    cardVm.Updated = true;
                }
            }
            // Remove all non-updated cards
            // TODO: This is where we could put an animation
            Board.RemoveAll(f => !f.Updated);


        }

        public void SetPlayableCards(IEnumerable<Homestead.Shared.PlayerAction> actions)
        {
            // Loop through each action and mark the cards as playable
            foreach (var action in actions)
            {
                if (action.Type == ActionType.Play)
                {
                    var cardVm = Hand.FirstOrDefault(f => f.Card == action.PlayerCard && !f.IsPlayable);
                    if (cardVm != null)
                    {
                        cardVm.IsPlayable = true;
                    }
                }
            }
        }
        public void ClearPlayableCards()
        {
            // Loop through each card and mark the cards as not playable
            foreach (var cardVm in Hand)
            {
                cardVm.IsPlayable = false;
            }
        }

    }
}
