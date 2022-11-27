using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homestead.Shared.Actions
{
    public class ActionDrawDeckCard : ActionBase
    {
        public ActionDrawDeckCard(Game game): base(game)
        {
            Type = PlayerAction.ActionType.DrawFromDeck;
        }
        
        public override void Run(PlayerAction action)
        {
            CurrentPlayer.Hand.Add(Cards.GetCard());

            // Determine what can happen next
            SetNextActionsFromHand();
        }
    }
}

