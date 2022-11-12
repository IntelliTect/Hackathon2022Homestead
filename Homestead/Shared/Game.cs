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

        public string GameId { get; set; }
        public GameState State { get; set; }
        public int? Winner { get; set; }
        public List<Player> Players { get; } = new();
        public List<string> DiscardPile { get; } = new List<string>();
        public int ActivePlayer { get; set; } = 1;
        public List<Action> Actions { get; } = new List<Action>();
        public Action? LastAction { get; set; } = null;
    }
}
