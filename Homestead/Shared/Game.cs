using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homestead.Shared
{
    public class Game
    {
        public enum GameState
        {
            Joining,
            Playing,
            Complete,
        }

        public string GameId { get; set; } = string.Empty;
        public GameState State { get; set; }
        public int? Winner { get; set; }
        public List<Player> Players { get; set; } = new();
        public List<string> DiscardPile { get; set; } = new ();
        public int ActivePlayer { get; set; } = 1;
        public List<PlayerAction> AvailableActions { get; set; } = new ();
        public List<PlayerAction> LastActions { get; set; } = new();
    }
}
