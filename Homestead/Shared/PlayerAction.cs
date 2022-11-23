using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

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

        public string ToString(Game game)
        {
            string result = $"Player {PlayerNumber}: {game.Players[PlayerNumber - 1].Name} ";
            result += $"{(game.Players[PlayerNumber - 1].IsBot ? "the bot " : "")}";
            switch (Type)
            {
                case PlayerAction.ActionType.DrawFromDeck:
                    result += "drew from the deck";
                    break;
                case PlayerAction.ActionType.DrawFromDiscard:
                    result += "drew from the discard pile";
                    break;
                case PlayerAction.ActionType.Play:
                    result += $"played the card '{PlayerCard}'";
                    break;
                case PlayerAction.ActionType.EndTurn:
                    result += "ended their turn";
                    break;
                case PlayerAction.ActionType.Discard:
                    result += $"discarded a {PlayerCard}";
                    break;
                default:
                    result += "did something interesting";
                    break;
            }
            return result;
        }
    }
}
