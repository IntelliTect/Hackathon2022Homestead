using Homestead.Shared;

namespace Homestead.Client.ViewModels
{
    public class CardVm
    {
        public string Card { get; }
        public CardInfo CardInfo { get;  }
        /// <summary>
        /// Internal use by UI only. Used to determine if the card has been updated when updating a player.
        /// </summary>
        internal bool Updated { get; set; }
        
        public bool IsPlayable { get; set; }

        public string ImageUrl => $"/Assets/Images/cards/{CardInfo.ImageFilename}";
        public PlayerVm Player { get; set; }

        public CardVm(string card, PlayerVm player)
        {
            Card = card;
            Updated = true;  // Set this right away so we don't use it again
            CardInfo = Cards.GetCardInfo(card);
            Player = player;
        }
        
    }
}
