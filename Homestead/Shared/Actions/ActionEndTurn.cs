using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homestead.Shared.Actions
{
    public class ActionEndTurn : ActionBase
    {
        public ActionEndTurn(Game game): base(game)
        {
            Type = PlayerAction.ActionType.EndTurn;
        }
        
        public override void Run(PlayerAction action)
        {
            if (Game.ActivePlayer < 4)
            {
                Game.ActivePlayer++;
            }
            else
            {
                Game.ActivePlayer = 1;
            }

            // Determine what can happen next
            SetNextActionsFromHand();
        }
    }
}

