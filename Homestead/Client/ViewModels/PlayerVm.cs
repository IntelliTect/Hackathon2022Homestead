using Homestead.Shared;
using System.Text;
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
        public bool IsCurrentPlayer { get; internal set; }
        public bool IsBot { get; internal set; }

        public List<PlayerCardVm> Hand
        { get; } = new();
        public List<PlayerCardVm> Board { get; } = new();

        public IEnumerable<PlayerCardVm> BoardLivestock => Board.Where(f => f.CardInfo.Suit == CardInfo.CardSuit.LiveStock);
        public IEnumerable<PlayerCardVm> BoardGarden => Board.Where(f => f.CardInfo.Suit == CardInfo.CardSuit.Garden);
        public IEnumerable<PlayerCardVm> BoardHouse => Board.Where(f => f.CardInfo.Suit == CardInfo.CardSuit.House);


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
            IsBot = player.IsBot;
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
                    Hand.Add(new PlayerCardVm(card, this));
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
                    Board.Add(new PlayerCardVm(card, this));
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
                else if (action.Type == ActionType.Discard)
                {
                    foreach (var card in Hand)
                    {
                        card.IsDiscardable = true;
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
                cardVm.IsDiscardable = false;
            }
        }

        public string GetImageUrl()
        {
            string genderCode = Player.Gender == Player.Genders.Male ? "m" : "f";
            string skinToneCode = Player.SkinTone == Player.SkinTones.Light ? "w" : "b";
            string fileName = $"p{PlayerNumber}-{genderCode}{skinToneCode}-single.png";
            return $"/Assets/Images/player/{fileName}";
        }

        public override string ToString()
        {
            var output = new StringBuilder();
            output.Append($"{PlayerNumber}:");
            output.Append(IsCurrentPlayer ? "Active" : "Waiting");
            output.Append("  ");
            output.Append($"Hand: {string.Join(", ", Hand.Select(f => f.Card))}");
            output.Append("  ");
            output.Append($"Board: {string.Join(", ", Board.Select(f => f.Card))}");
            return output.ToString();
        }
    }
}
