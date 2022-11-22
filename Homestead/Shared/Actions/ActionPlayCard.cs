using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace Homestead.Shared.Actions
{
    public class ActionPlayCard : ActionBase
    {
        public ActionPlayCard(Game game) : base(game)
        {
            Type = PlayerAction.ActionType.Play;
        }

        public override void Run(PlayerAction action)
        {
            if (action.PlayerCard is not null)
            {
                // Make sure the player has the card
                if (CurrentPlayer.Hand.Contains(action.PlayerCard))
                {
                    switch (Cards.GetCardInfo(action.PlayerCard).Suit)
                    {
                        case CardInfo.CardSuit.House:
                            PlayCardToBoard(action.PlayerCard); break;
                        case CardInfo.CardSuit.LiveStock:
                            PlayCardToBoard(action.PlayerCard); break;
                        case CardInfo.CardSuit.Garden: PlayCardToBoard(action.PlayerCard); break;
                        case CardInfo.CardSuit.Action: PerformAction(action.PlayerCard); break;
                        case CardInfo.CardSuit.Disaster: DoDisaster(action.PlayerCard); break;
                        default: throw new ArgumentException("Invalid card");
                    }
                }
                else
                {
                    throw new KeyNotFoundException("Player doesn't have that card.");
                }
            }
            // Determine what can happen next
            SetNextActionsFromHand();
        }

        private void PlayCardToBoard(string card)
        {
            CurrentPlayer.Board.Add(card);
            CurrentPlayer.Hand.Remove(card);
            SetNextActionsFromHand();
        }
        private void PerformAction(string card)
        {
            // TODO: do stuff for each card
            // Check which type of action it is and then add next actions


            CurrentPlayer.Hand.Remove(card);
        }
        private void DoDisaster(string card)
        {
            // TODO: do the disaster
            // Check which type of action it is and then add next actions



            CurrentPlayer.Hand.Remove(card);
        }
    }
}

