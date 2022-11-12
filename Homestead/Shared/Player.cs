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

        public int PlayerNumber { get;  }
        public bool IsBot { get; set; }
        public string Name { get; set; }
        public SkinTones SkinTone { get; set; }
        public Genders Gender { get; set; }
        public List<string> Hand { get; } = new List<string>();
        public List<string> Board { get; } = new List<string>();


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
    }
}
