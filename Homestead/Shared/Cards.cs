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
            MasterDeck.Add(new CardInfo(EarthquakeSelf, "Quake: Me",CardInfo.CardSuit.Disaster, false, CardInfo.CardImpact.Self, "earthquake.png", few, Cards.Well));
            MasterDeck.Add(new CardInfo(EarthquakeOther, "Quake: You", CardInfo.CardSuit.Disaster, false, CardInfo.CardImpact.Other, "earthquake.png", few, Cards.Well));
            MasterDeck.Add(new CardInfo(EarthquakeAll, "Quake: All",CardInfo.CardSuit.Disaster, false, CardInfo.CardImpact.All, "earthquake.png", few, Cards.Well));
            MasterDeck.Add(new CardInfo(FireSelf, "Fire: Me", CardInfo.CardSuit.Disaster, false, CardInfo.CardImpact.Self, "fire.png", few, Cards.Wood, Cards.Rain));
            MasterDeck.Add(new CardInfo(FireOther, "Fire: You", CardInfo.CardSuit.Disaster, false, CardInfo.CardImpact.Other, "fire.png", few, Cards.Wood, Cards.Rain));
            MasterDeck.Add(new CardInfo(FireAll, "Fire: All", CardInfo.CardSuit.Disaster, false, CardInfo.CardImpact.All, "fire.png", few, Cards.Wood, Cards.Rain));
            MasterDeck.Add(new CardInfo(FloodSelf, "Flood: Me", CardInfo.CardSuit.Disaster, false, CardInfo.CardImpact.Self, "flood.png", few, Cards.Seeds, Cards.Levee));
            MasterDeck.Add(new CardInfo(FloodOther, "Flood: You", CardInfo.CardSuit.Disaster, false, CardInfo.CardImpact.Other, "flood.png", few, Cards.Seeds, Cards.Levee));
            MasterDeck.Add(new CardInfo(FloodAll, "Flood: All", CardInfo.CardSuit.Disaster, false, CardInfo.CardImpact.All, "flood.png", few, Cards.Seeds, Cards.Levee));
            MasterDeck.Add(new CardInfo(WolfSelf, "Wolf: Me", CardInfo.CardSuit.Disaster, false, CardInfo.CardImpact.Self, "wolf.png", few, Cards.Livestock, Cards.Dog));
            MasterDeck.Add(new CardInfo(WolfOther, "Wolf: You", CardInfo.CardSuit.Disaster, false, CardInfo.CardImpact.Other, "wolf.png", few, Cards.Livestock, Cards.Dog));
            MasterDeck.Add(new CardInfo(WolfAll, "Wolf: All", CardInfo.CardSuit.Disaster, false, CardInfo.CardImpact.All, "wolf.png", few, Cards.Livestock, Cards.Dog));

            MasterDeck.Add(new CardInfo(GoodNeighbor, "Give", CardInfo.CardSuit.Action, false, CardInfo.CardImpact.None, "good-neighbor", some));
            MasterDeck.Add(new CardInfo(BadNeighbor, "Steal", CardInfo.CardSuit.Action, false, CardInfo.CardImpact.None, "bad-neighbor", some));

            MasterDeck.Add(new CardInfo(Levee, "Levee", CardInfo.CardSuit.Prevention, false, CardInfo.CardImpact.None, "levee.png", some));
            MasterDeck.Add(new CardInfo(Dog, "D.O.G.", CardInfo.CardSuit.Prevention, false, CardInfo.CardImpact.None, "dog.png", lots));
            MasterDeck.Add(new CardInfo(Rain, "Rain", CardInfo.CardSuit.Prevention, false, CardInfo.CardImpact.None, "rain.png", some));
            
            MasterDeck.Add(new CardInfo(Livestock, "Animals", CardInfo.CardSuit.LiveStock, true, CardInfo.CardImpact.None, "livestock.png", lots));
            MasterDeck.Add(new CardInfo(Seeds, "Seeds", CardInfo.CardSuit.Garden, true, CardInfo.CardImpact.None, "seeds.png", lots));
            MasterDeck.Add(new CardInfo(Well, "Well", CardInfo.CardSuit.Garden, true, CardInfo.CardImpact.None, "well.png", lots));
            MasterDeck.Add(new CardInfo(Shovel, "Shovel", CardInfo.CardSuit.Garden, true, CardInfo.CardImpact.None, "shovel.png", lots));
            MasterDeck.Add(new CardInfo(Saw, "Saw", CardInfo.CardSuit.House, true, CardInfo.CardImpact.None, "saw.png", lots));
            MasterDeck.Add(new CardInfo(Hammer, "Hammer", CardInfo.CardSuit.House, true, CardInfo.CardImpact.None, "hammer.png", lots));
            MasterDeck.Add(new CardInfo(Wood, "Wood", CardInfo.CardSuit.House, true, CardInfo.CardImpact.None, "wood-pile.png", lots));
            MasterDeck.Add(new CardInfo(Stove, "Stove", CardInfo.CardSuit.House, true, CardInfo.CardImpact.None, "wood-stove.png", lots));

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
        public string? ImpactedCard { get; }
        public string? PreventionCard { get; }
        public string Name { get; }

        public CardInfo(string card,
            string name,
            CardSuit suit,
            bool requiredToWin,
            CardImpact impact,
            string imageFilename,
            int numberOfCardsInDeck,
            string? impactedCard = null,
            string? preventionCard = null)
        {
            Card = card;
            Name = name;
            Suit = suit;
            RequiredToWin = requiredToWin;
            Impact = impact;
            ImageFilename = imageFilename;
            ImpactedCard = impactedCard;
            PreventionCard = preventionCard;
            NumberOfCardsInDeck = numberOfCardsInDeck;
        }
    }
}
