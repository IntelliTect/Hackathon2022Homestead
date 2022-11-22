using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homestead.Shared
{
    public class PlayerAction
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

        public string? PlayerCard { get; set; }
        public int? TargetPlayer { get; set; }
        public string? TargetCard { get; set; }
        // TODO: Set these up on the server and the client.
        public bool PlayerSelectionRequired { get; set; }
        public bool PlayerCardSelectionRequired { get; set; }
        
        public PlayerAction(ActionType type, int playerNumber, string? PlayerCard = null, int? TargetPlayer = null, string? TargetCard = null)
        {
            this.Type = type;
            this.PlayerNumber = playerNumber;
            this.PlayerCard = PlayerCard;
            this.TargetPlayer = TargetPlayer;
            this.TargetCard = TargetCard;
        }
    }
}
