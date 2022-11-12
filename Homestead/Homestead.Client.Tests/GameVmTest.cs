using Homestead.Client.ViewModels;
using Homestead.Shared;

namespace Homestead.Client.Tests
{
    [TestClass]
    public class GameVmTest
    {
        [TestMethod]
        public void Create()
        {
            var game = (new GameEngine()).Start();

            var board = new BoardVm(game,2);
            Assert.AreEqual(2, board.LocalPlayer.PlayerNumber);
            Assert.AreEqual(1, board.OtherPlayers[0].PlayerNumber);
            Assert.AreEqual(3, board.OtherPlayers[1].PlayerNumber);
            Assert.AreEqual(4, board.OtherPlayers[2].PlayerNumber);
        }

        public void UpdateWhenPlayersTurn()
        {
            var game = (new GameEngine()).Start();
            var board = new BoardVm(game, 2);

            // Update the game
            game.ActivePlayer = 2;
            game.AvailableActions.Add(new PlayerAction(PlayerAction.ActionType.Discard, 2));
            game.AvailableActions.Add(new PlayerAction(PlayerAction.ActionType.DrawFromDiscard, 2));
            game.AvailableActions.Add(new PlayerAction(PlayerAction.ActionType.DrawFromDiscard, 2));
            game.AvailableActions.Add(new PlayerAction(PlayerAction.ActionType.DrawFromDiscard, 2));
            game.Players[1].Hand.Add(Cards.BadNeighbor);
            game.AvailableActions.Add(new PlayerAction(PlayerAction.ActionType.Play, 2, Cards.BadNeighbor));
            
            board.Update(game);

            Assert.IsTrue(board.CanDiscard);
            Assert.IsTrue(board.CanDrawFromDeck);
            Assert.IsTrue(board.CanDrawFromDiscard);
            Assert.IsTrue(board.CanEndTurn);

            foreach (var card in board.LocalPlayer.Hand)
            {
                if (card.Card == Cards.BadNeighbor)
                {
                    Assert.IsTrue(card.IsPlayable);
                }
                else
                {
                    Assert.IsFalse(card.IsPlayable);
                }
            }
        }

        public void UpdateWhenNotPlayersTurn()
        {
            var game = (new GameEngine()).Start();
            var board = new BoardVm(game, 2);
            
            // Update the game
            game.ActivePlayer = 1;
            game.AvailableActions.Add(new PlayerAction(PlayerAction.ActionType.Discard, 1));
            game.AvailableActions.Add(new PlayerAction(PlayerAction.ActionType.DrawFromDiscard, 1));
            game.AvailableActions.Add(new PlayerAction(PlayerAction.ActionType.DrawFromDiscard, 1));
            game.AvailableActions.Add(new PlayerAction(PlayerAction.ActionType.DrawFromDiscard, 1));
            game.Players[1].Hand.Add(Cards.BadNeighbor);
            game.AvailableActions.Add(new PlayerAction(PlayerAction.ActionType.Play, 2, Cards.BadNeighbor));

            board.Update(game);

            Assert.IsFalse(board.CanDiscard);
            Assert.IsFalse(board.CanDrawFromDeck);
            Assert.IsFalse(board.CanDrawFromDiscard);
            Assert.IsFalse(board.CanEndTurn);
            
            foreach(var card in board.LocalPlayer.Hand)
            {
                Assert.IsFalse(card.IsPlayable);
            }
        }

    }
}