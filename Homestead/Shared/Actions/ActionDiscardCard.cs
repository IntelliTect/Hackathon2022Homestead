using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homestead.Shared.Actions
{
    public class ActionDiscardCard : ActionBase
    {
        public ActionDiscardCard(Game game): base(game)
        {
            Type = PlayerAction.ActionType.Discard;
        }
        
        public override void Run(PlayerAction action)
        {
            if (action.PlayerCard is not null)
            {
                CurrentPlayer.Hand.Remove(action.PlayerCard);
                Game.DiscardPile.Add(action.PlayerCard);
            }
            else
            {
                throw new Exception("No card specified");
            }

            // Determine what can happen next
            SetNextActionsFromHand();
        }
    }
}

