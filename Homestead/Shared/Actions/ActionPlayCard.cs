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
            Game.AvailableActions.Clear();
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
                        case CardInfo.CardSuit.Disaster: DoDisaster(action.PlayerCard, action.TargetPlayer); break;
                        default: throw new ArgumentException("Invalid card");
                    }
                }
                else
                {
                    throw new KeyNotFoundException("Player doesn't have that card.");
                }
            }
            // If no actions are set, determine what can happen next
            if (!Game.AvailableActions.Any())
            {
                SetNextActionsFromHand();
            }
            Game.LastPlayedCard = action.PlayerCard;
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
        private void DoDisaster(string card, int? targetPlayer)
        {
            var impactedCard = (Cards.GetCardInfo(card).ImpactedCard);
            var preventionCard = (Cards.GetCardInfo(card).PreventionCard);

            if (impactedCard != null)
            {

                switch (Cards.GetCardInfo(card).Impact)
                {
                    case CardInfo.CardImpact.Self:
                        // See if I have the impacted card and remove it
                        RemoveCardsBasedOnDisaster(Game.ActivePlayer, impactedCard, preventionCard);
                        break;
                    case CardInfo.CardImpact.All:
                        foreach (var player in Game.Players)
                        {
                            RemoveCardsBasedOnDisaster(player.PlayerNumber, impactedCard, preventionCard);
                        }
                        break;
                    case CardInfo.CardImpact.Other:
                        if (targetPlayer != null)
                        {
                            RemoveCardsBasedOnDisaster(targetPlayer.Value, impactedCard, preventionCard);
                        }
                        else
                        {
                            throw new ArgumentException("No target player specified");
                        }
                        break;
                    default:
                        throw new ArgumentException("Wrong impact");
                }
                // TODO: do the disaster
                // Check which type of action it is and then add next actions
            }
            else
            {
                throw new ArgumentException("Disaster doesn't have impacted card");
            }

            CurrentPlayer.Hand.Remove(card);
        }

        private bool RemoveCardsBasedOnDisaster(int playerNumber, string impactedCard, string preventionCard)
        {
            if (preventionCard==null || !RemoveCardFromPlayersHand(playerNumber, preventionCard))
            {
                return RemoveCardFromPlayersBoard(playerNumber, impactedCard);
            }
            return true;
        }

        private bool RemoveCardFromPlayersBoard(int playerNumber, string card)
        {

            // Remove the impacted card from the board
            if (Game.Players[playerNumber - 1].Board.Contains(card))
            {
                Game.Players[playerNumber - 1].Board.Remove(card);
                return true;
            }
            return false;
        }
        private bool RemoveCardFromPlayersHand(int playerNumber, string card)
        {
            if (Game.Players[playerNumber - 1].Hand.Contains(card))
            {
                Game.Players[playerNumber - 1].Hand.Remove(card);
                return true;
            }
            return false;
        }
    }
}

