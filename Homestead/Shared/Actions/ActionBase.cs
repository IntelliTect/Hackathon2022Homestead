using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Homestead.Shared.PlayerAction;

namespace Homestead.Shared.Actions
{
    public abstract class ActionBase
    {
        public Game Game { get; }
        public ActionBase(Game game)
        {
            Game = game;
        }

        public PlayerAction.ActionType Type { get; internal set; }

        public abstract void Run(PlayerAction action);

        internal Player CurrentPlayer
        {
            get
            {
                return Game.Players[Game.ActivePlayer - 1];
            }
        }

        /// <summary>
        /// Call this when the player can play from their hand or end their turn.
        /// </summary>
        internal void SetNextActionsFromHand()
        {
            ClearActions();
            // See if the player has a new disaster card and force them to play it.
            foreach (var card in CurrentPlayer.Hand.Where(f => Cards.GetCardInfo(f).Suit == CardInfo.CardSuit.Disaster))
            {
                AddAction(PlayerAction.ActionType.Play, card);
            }

            // If there are no required disaster card plays then see if there is a forced play to the board.
            if (!Game.AvailableActions.Any())
            {
                // See if the player is forced to play a card
                // Also check for possible discards
                List<string> discards = new();
                foreach (var card in CurrentPlayer.Hand.Where(f => Cards.GetCardInfo(f).RequiredToWin))
                {
                    if (!CurrentPlayer.Board.Contains(card))
                    {
                        AddAction(PlayerAction.ActionType.Play, card);
                    }
                    else
                    {
                        discards.Add(card);
                    }
                }
                // There are no required plays see about optional ones.
                if (!Game.AvailableActions.Any())
                {
                    // Add the valid discards
                    foreach (var card in discards)
                    {
                        AddAction(PlayerAction.ActionType.Discard, card);
                    }
                    // See if there are action plays
                    foreach (var card in CurrentPlayer.Hand.Where(f => Cards.GetCardInfo(f).Suit == CardInfo.CardSuit.Action))
                    {
                        AddAction(PlayerAction.ActionType.Play, card);
                    }
                    // The player should also be able to end their turn if they don't have 5 cards
                    if (CurrentPlayer.Hand.Count < 5)
                    {
                        AddAction(ActionType.EndTurn);
                    }
                }
            }
        }

        internal void AddAction(ActionType type, string? PlayerCard = null, int? TargetPlayer = null, string? TargetCard = null)
        {
            Game.AvailableActions.Add(new PlayerAction(type, Game.ActivePlayer, PlayerCard, TargetPlayer, TargetCard));
        }
        internal void ClearActions()
        {
            Game.AvailableActions.Clear();
        }
    }
}

