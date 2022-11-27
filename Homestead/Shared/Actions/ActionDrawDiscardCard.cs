using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homestead.Shared.Actions
{
    public class ActionDrawDiscardCard : ActionBase
    {
        public ActionDrawDiscardCard(Game game): base(game)
        {
            Type = PlayerAction.ActionType.DrawFromDeck;
        }
        
        public override void Run(PlayerAction action)
        {
            if (Game.DiscardPile.Count > 0)
            {
                var card = Game.DiscardPile[Game.DiscardPile.Count - 1];
                Game.DiscardPile.RemoveAt(Game.DiscardPile.Count - 1);
                CurrentPlayer.Hand.Add(card);
            }
            else
            {
                throw new Exception("Invalid Action, no cards in discard pile");
            }

            // Determine what can happen next
            SetNextActionsFromHand();
        }
    }
}

