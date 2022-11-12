using Homestead.Shared;

namespace Homestead.Client.ViewModels
{
    public class CardVm
    {
        public string Card { get; }
        public CardInfo CardInfo { get; }

        public string ImageUrl => $"/Assets/Images/cards/{CardInfo.ImageFilename}";

        public CardVm(string card)
        {
            Card = card;
            CardInfo = Cards.GetCardInfo(card);
        }
    }

    public class PlayerCardVm: CardVm
    {
        public PlayerVm Player { get; set; }
        public bool IsPlayable { get; set; }
        public bool IsDiscardable { get; set; }
        /// <summary>
        /// Internal use by UI only. Used to determine if the card has been updated when updating a player.
        /// </summary>
        internal bool Updated { get; set; }

        public PlayerCardVm(string card, PlayerVm player): base(card)
        {
            Player = player;
            Updated = true;  // Set this right away so we don't use it again
        }
    }
}
