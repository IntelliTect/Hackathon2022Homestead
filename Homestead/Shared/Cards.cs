using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homestead.Shared
{
    public static class Cards
    {
        public const string Well = "Well";
        public const string Dog = "Dog";
        public const string EarthquakeSelf = "EarthquakeSelf";
        public const string EarthquakeOther = "EarthquakeOther";
        public const string EarthquakeAll = "EarthquakeAll";
        public const string FireSelf = "FireSelf";
        public const string FireOther = "FireOther";
        public const string FireAll = "FireAll";
        public const string FloodSelf = "FloodSelf";
        public const string FloodOther = "FloodOther";
        public const string FloodAll = "FloodAll";
        public const string WolfSelf = "WolfSelf";
        public const string WolfOther = "WolfOther";
        public const string WolfAll = "WolfAll";
        public const string GoodNeighbor = "GoodNeighbor";
        public const string Hammer = "Hammer";
        public const string BadNeighbor = "BadNeighbor";
        public const string Levee = "Levee";
        public const string Livestock = "Livestock";
        public const string Rain = "Raid";
        public const string Saw = "Saw";
        public const string Seeds = "Seeds";
        public const string Shovel = "Shovel";
        public const string Wood = "Wood";
        public const string Stove = "Stove";

        private static List<CardInfo> MasterDeck { get; } = new List<CardInfo>();
        private static List<string> Deck { get; } = new List<string>();
        private static Random staticRandom = new Random(1);

        static Cards()
        {
            int lots = 5;
            int some = 3;
            int few = 1;
            MasterDeck.Add(new CardInfo(EarthquakeSelf, CardInfo.CardSuit.Disaster, false, CardInfo.CardImpact.Self, "earthquake.png", few));
            MasterDeck.Add(new CardInfo(EarthquakeOther, CardInfo.CardSuit.Disaster, false, CardInfo.CardImpact.Other, "earthquake.png", few));
            MasterDeck.Add(new CardInfo(EarthquakeAll, CardInfo.CardSuit.Disaster, false, CardInfo.CardImpact.All, "earthquake.png", few));
            MasterDeck.Add(new CardInfo(FireSelf, CardInfo.CardSuit.Disaster, false, CardInfo.CardImpact.Self, "fire.png", few));
            MasterDeck.Add(new CardInfo(FireOther, CardInfo.CardSuit.Disaster, false, CardInfo.CardImpact.Other, "fire.png", few));
            MasterDeck.Add(new CardInfo(FireAll, CardInfo.CardSuit.Disaster, false, CardInfo.CardImpact.All, "fire.png", few));
            MasterDeck.Add(new CardInfo(FloodSelf, CardInfo.CardSuit.Disaster, false, CardInfo.CardImpact.Self, "flood.png", few));
            MasterDeck.Add(new CardInfo(FloodOther, CardInfo.CardSuit.Disaster, false, CardInfo.CardImpact.Other, "flood.png", few));
            MasterDeck.Add(new CardInfo(FloodAll, CardInfo.CardSuit.Disaster, false, CardInfo.CardImpact.All, "flood.png", few));
            MasterDeck.Add(new CardInfo(WolfSelf, CardInfo.CardSuit.Disaster, false, CardInfo.CardImpact.Self, "wolf.png", lots));
            MasterDeck.Add(new CardInfo(WolfOther, CardInfo.CardSuit.Disaster, false, CardInfo.CardImpact.Other, "wolf.png", lots));
            MasterDeck.Add(new CardInfo(WolfAll, CardInfo.CardSuit.Disaster, false, CardInfo.CardImpact.All, "wolf.png", lots));

            MasterDeck.Add(new CardInfo(GoodNeighbor, CardInfo.CardSuit.Action, false, CardInfo.CardImpact.None, "good-neighbor", some));
            MasterDeck.Add(new CardInfo(BadNeighbor, CardInfo.CardSuit.Action, false, CardInfo.CardImpact.None, "bad-neighbor", some));

            MasterDeck.Add(new CardInfo(Levee, CardInfo.CardSuit.Prevention, false, CardInfo.CardImpact.None, "levee.png", some));
            MasterDeck.Add(new CardInfo(Dog, CardInfo.CardSuit.Prevention, false, CardInfo.CardImpact.None, "dog.png", lots));
            MasterDeck.Add(new CardInfo(Rain, CardInfo.CardSuit.Prevention, false, CardInfo.CardImpact.None, "rain.png", some));
            
            MasterDeck.Add(new CardInfo(Livestock, CardInfo.CardSuit.LiveStock, true, CardInfo.CardImpact.None, "livestock.png", lots));
            MasterDeck.Add(new CardInfo(Seeds, CardInfo.CardSuit.Garden, true, CardInfo.CardImpact.None, "seeds.png", lots));
            MasterDeck.Add(new CardInfo(Well, CardInfo.CardSuit.Garden, true, CardInfo.CardImpact.None, "well.png", lots));
            MasterDeck.Add(new CardInfo(Shovel, CardInfo.CardSuit.Garden, true, CardInfo.CardImpact.None, "shovel.png", lots));
            MasterDeck.Add(new CardInfo(Saw, CardInfo.CardSuit.House, true, CardInfo.CardImpact.None, "saw.png", lots));
            MasterDeck.Add(new CardInfo(Hammer, CardInfo.CardSuit.House, true, CardInfo.CardImpact.None, "hammer.png", lots));
            MasterDeck.Add(new CardInfo(Wood, CardInfo.CardSuit.House, true, CardInfo.CardImpact.None, "wood.png", lots));
            MasterDeck.Add(new CardInfo(Stove, CardInfo.CardSuit.House, true, CardInfo.CardImpact.None, "stove.png", lots));

            // Create a deck to choose cards from
            foreach (var cardRatio in MasterDeck)
            {
                for (int i = 0; i < cardRatio.NumberOfCardsInDeck; i++)
                {
                    Deck.Add(cardRatio.Card);
                }
            }
        }

        /// <summary>
        /// Gets info about a card.
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public static CardInfo GetCardInfo(string card)
        {
            return MasterDeck.First(f => f.Card == card);
        }

        public static string GetCard(bool deterministic = false)
        {
            Random random;
            if (deterministic)
            {
                random = staticRandom;
            }
            else
            {
                random = new Random();
            }
            return Deck[random.Next(Deck.Count)];
        }
    }

    public class CardInfo
    {
        public enum CardSuit
        {
            Garden,
            House,
            LiveStock,
            Action,
            Prevention,
            Disaster,
        }

        public enum CardImpact
        {
            None,
            Self,
            All,
            Other,
        }

        public string Card { get; }
        public int NumberOfCardsInDeck { get; }
        public CardSuit Suit { get; }
        public bool RequiredToWin { get; }
        public CardImpact Impact { get; }
        public string ImageFilename { get; }
        public string ImageUrl => $"/Assets/Images/cards/{ImageFilename}";

        public CardInfo(string card, CardSuit suit, bool requiredToWin, CardImpact impact, string imageFilename, int numberOfCardsInDeck)
        {
            Card = card;
            Suit = suit;
            RequiredToWin = requiredToWin;
            Impact = impact;
            ImageFilename = imageFilename;
            NumberOfCardsInDeck = numberOfCardsInDeck;
        }
    }
}
