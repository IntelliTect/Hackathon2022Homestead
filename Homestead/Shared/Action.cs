using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homestead.Shared
{
    public class Action
    {
        public enum ActionType
        {
            DrawFromDeck,
            DrawFromDiscard,
            Play,
            Discard,
            EndTurn
        }

        public int PlayerNumber{ get; set; }

        public ActionType Type { get; set; }

        public string? Card { get; set; }
        public int? TargetPlayer { get; set; }
        public string? TargetCard { get; set; }

        public Action(ActionType type, int playerNumber)
        {
            Type = type;
            PlayerNumber = playerNumber;
        }
    }
}
