using RandomNameGeneratorLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homestead.Shared
{
    public class Player
    {
        public enum SkinTones
        {
            Light,
            Dark
        }
        public enum Genders
        {
            Male,
            Female
        }

        public int PlayerNumber { get; set; }
        public bool IsBot { get; set; }
        public string Name { get; set; }
        public SkinTones SkinTone { get; set; }
        public Genders Gender { get; set; }
        public List<string> Hand { get; } = new List<string>();
        public List<string> Board { get; } = new List<string>();

        /// <summary>
        /// This should only be called by the SignalR deserializer
        /// </summary>
        public Player()
        {
            PlayerNumber = 0;
            Name = "";
        }

        public string GetPlayerImageUrl()
        {
            string genderCode = Gender == Genders.Male ? "m" : "f";
            string skinToneCode = SkinTone == SkinTones.Light ? "w" : "b";
            string fileName = $"p{PlayerNumber}-{genderCode}{skinToneCode}-single.png";
            return $"/Assets/Images/player/{fileName}";
        }

    /// <summary>
    /// This is called by the Game when it gets created
    /// </summary>
    /// <param name="playerNumber"></param>
    public Player(int playerNumber)
        {
            PlayerNumber = playerNumber;
            var personGenerator = new PersonNameGenerator();
            Name = personGenerator.GenerateRandomFirstName();

            // Pick random enums
            Random random = new Random();
            Array values = Enum.GetValues(typeof(SkinTones));
            SkinTone = (SkinTones)(values.GetValue(random.Next(values.Length))!);

            values = Enum.GetValues(typeof(Genders));
            Gender = (Genders)(values.GetValue(random.Next(values.Length))!);
        }

        public IEnumerable<string> BoardBySuit(CardInfo.CardSuit suit) => Board.Where(f => Cards.GetCardInfo(f).Suit == suit);

    }
}
