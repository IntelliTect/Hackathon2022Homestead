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
        public const string GoodNeighbor = "GoodNeighbor";
        public const string Hammer = "Hammer";
        public const string BadNeighbor = "BadNeighbor";
        public const string Levee = "Levee";
        public const string Livestock = "Livestock";
        public const string Rain = "Raid";
        public const string Saw = "Saw";
        public const string Seeds = "Seeds";
        public const string Shovel = "Shovel";
        public const string Wolf = "Wolf";
        public const string Wood = "Wood";
        public const string Stove = "Stove";

        private static List<CardRatio> MasterDeck { get; } = new List<CardRatio>();
        private static List<string> Deck { get; } = new List<string>();
        private static Random staticRandom = new Random(1);

        static Cards()
        {
            int lots = 5;
            int some = 3;
            int few = 1;
            MasterDeck.Add(new CardRatio(Well, lots));
            MasterDeck.Add(new CardRatio(Dog, lots));
            MasterDeck.Add(new CardRatio(EarthquakeSelf, few));
            MasterDeck.Add(new CardRatio(EarthquakeOther, few));
            MasterDeck.Add(new CardRatio(EarthquakeAll, few));
            MasterDeck.Add(new CardRatio(FireSelf, few));
            MasterDeck.Add(new CardRatio(FireOther, few));
            MasterDeck.Add(new CardRatio(FireAll, few));
            MasterDeck.Add(new CardRatio(FloodSelf, few));
            MasterDeck.Add(new CardRatio(FloodOther, few));
            MasterDeck.Add(new CardRatio(FloodAll, few));
            MasterDeck.Add(new CardRatio(GoodNeighbor, some));
            MasterDeck.Add(new CardRatio(Hammer, lots));
            MasterDeck.Add(new CardRatio(BadNeighbor, some));
            MasterDeck.Add(new CardRatio(Levee, some));
            MasterDeck.Add(new CardRatio(Livestock, lots));
            MasterDeck.Add(new CardRatio(Rain, some));
            MasterDeck.Add(new CardRatio(Saw, lots));
            MasterDeck.Add(new CardRatio(Seeds, lots));
            MasterDeck.Add(new CardRatio(Shovel, lots));
            MasterDeck.Add(new CardRatio(Wolf, lots));
            MasterDeck.Add(new CardRatio(Wood, lots));
            MasterDeck.Add(new CardRatio(Stove, lots));

            // Create a deck to choose cards from
            foreach(var cardRatio in MasterDeck)
            {
                for(int i = 0; i < cardRatio.NumberOfCardsInDeck; i++)
                {
                    Deck.Add(cardRatio.Card);
                }
            }
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

    public class CardRatio
    {
        public string Card { get; }
        public int NumberOfCardsInDeck { get; }

        public CardRatio(string card, int numberOfCardsInDeck)
        {
            Card = card;
            NumberOfCardsInDeck = numberOfCardsInDeck;
        }
    }
}
