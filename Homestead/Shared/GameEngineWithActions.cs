using Homestead.Shared.Actions;
using static Homestead.Shared.PlayerAction;

namespace Homestead.Shared
{
    public class GameEngineWithActions : IGameEngine
    {
        /// <summary>
        /// Creates a new game instance and returns it ready for play.<br />
        /// Assumes the active player is player 1 with the only available actions as Draw from Deck.<br />
        /// Player 1 must not be a bot.
        /// </summary>
        /// <returns></returns>
        public Game Start(string playerId)
        {
            Game game = new();
            game.GameId = Guid.NewGuid().ToString();
            const int firstPlayer = 1;

            // Create 4 players
            for (int i = firstPlayer; i <= 4; i++)
            {
                var player = new Player(i);
                if (i == firstPlayer)
                {
                    player.PlayerId = playerId;
                }
                else
                {
                    player.IsBot = true;
                }
                game.Players.Add(player);
            }
            game.ActivePlayer = firstPlayer;
            game.AvailableActions.Add(new PlayerAction(PlayerAction.ActionType.DrawFromDeck, firstPlayer));
            return game;
        }

        private static ActionBase GetAction(ActionType type, Game game)
        {
            switch (type)
            {
                case ActionType.Play:
                    return new ActionPlayCard(game);
                case ActionType.Discard:
                    return new ActionDiscardCard(game);
                case ActionType.DrawFromDiscard:
                    return new ActionDrawDiscardCard(game);
                case ActionType.DrawFromDeck:
                    return new ActionDrawDeckCard(game);
                case ActionType.EndTurn:
                    return new ActionEndTurn(game);
                default:
                    // TODO: Make this better handled on the client
                    throw new ArgumentException($"No action found for enum {type}");
            }
        }

        /// <summary>
        /// Takes an action and updates the game state of the game passed in
        /// </summary>
        /// <param name="game"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        public Game ProcessAction(Game game, PlayerAction action)
        {
            ActionBase actionRunner = GetAction(action.Type, game);
            game.LastActions.Clear();
            if (actionRunner != null)
            {
                actionRunner.Run(action);
            }
            game.LastActions.Add(action);

            // Check and Set winner
            CheckAndSetWinner(game);

            game.LastPlayDate = DateTime.UtcNow;
            return game;
        }


        private void CheckAndSetWinner(Game game)
        {
            foreach (var player in game.Players)
            {
                if (player.Board.Count(c => Cards.GetCardInfo(c).RequiredToWin) == 8)
                {
                    game.State = Game.GameState.Complete;
                    game.Winner = game.ActivePlayer;
                    game.AvailableActions.Clear();
                }
            }
        }
    }
}
