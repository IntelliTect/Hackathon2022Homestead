using NuGet.Frameworks;

namespace Homestead.Shared.Tests
{
    [TestClass]
    public class GameEngineTest
    {
        // Do test setup!
        [TestMethod]
        public void StartGameTest()
        { 
            // break this test up
            GameEngine engine = new();
            Game game = engine.Start();
            Assert.IsNotNull(game);
            Assert.IsFalse(string.IsNullOrEmpty(game.GameId));
            Assert.AreEqual(1, game.ActivePlayer);
            Assert.IsTrue(game.AvailableActions.Any());
            //CollectionAssert.Contains(game.Actions, Action.ActionType.DrawFromDeck);
            Assert.AreEqual(Action.ActionType.DrawFromDeck, game.AvailableActions[0].Type);
            Assert.AreEqual(1, game.AvailableActions[0].PlayerNumber);
        }


        [TestMethod]
        public void DrawFromDeckTest()
        {
            GameEngine engine = new();
            Game game = engine.Start();

            game = engine.ProcessAction(game, new Action(Action.ActionType.DrawFromDeck, game.ActivePlayer));
            Assert.IsTrue(game.Players[game.ActivePlayer].Hand.Any());
        }

        [TestMethod]
        public void DiscardCardTest()
        {
            GameEngine engine = new();
            Game game = engine.Start();

            Action action = new(Action.ActionType.Discard, game.ActivePlayer);
            action.PlayerCard = Cards.Well;

            game.Players[game.ActivePlayer].Hand.Add(Cards.Well);

            Assert.IsTrue(game.Players[game.ActivePlayer].Hand.Any());
            game = engine.ProcessAction(game, action);
            Assert.IsFalse(game.Players[game.ActivePlayer].Hand.Any());
        }

        // Test for discarding a card not in your hand
        // Test for trying to discard with no cards in hand

        [TestMethod]
        public void DrawFromDiscardTest()
        {
            GameEngine engine = new();
            Game game = engine.Start();

            Action action = new(Action.ActionType.DrawFromDiscard, game.ActivePlayer);
            game.DiscardPile.Add(Cards.Well);

            game = engine.ProcessAction(game, action);
            Assert.IsFalse(game.DiscardPile.Any());
            Assert.IsTrue(game.Players[game.ActivePlayer].Hand.Any());
            Assert.IsTrue(game.Players[game.ActivePlayer].Hand[0] is Cards.Well);
        }

        [TestMethod]
        public void DrawFromDiscardWhenMultipleOfSameTypeExist()
        {
            GameEngine engine = new();
            Game game = engine.Start();

            Action action = new(Action.ActionType.DrawFromDiscard, game.ActivePlayer);
            game.DiscardPile.Add(Cards.Well);
            game.DiscardPile.Add(Cards.Seeds);
            game.DiscardPile.Add(Cards.Wood);
            game.DiscardPile.Add(Cards.Well);

            game = engine.ProcessAction(game, action);
            Assert.IsTrue(game.Players[game.ActivePlayer].Hand.Any());
            Assert.IsTrue(game.Players[game.ActivePlayer].Hand[0] is Cards.Well);

            Assert.IsTrue(game.DiscardPile.Any());
            Assert.IsTrue(game.DiscardPile.First() is Cards.Well);
            Assert.IsTrue(game.DiscardPile.Last() is Cards.Wood);
        }

        // Draw from discard with no discard

        [TestMethod]
        public void PlayCard()
        {
            GameEngine engine = new();
            Game game = engine.Start();

            Action action = new(Action.ActionType.Play, game.ActivePlayer);
            action.PlayerCard = Cards.Well;
            game = engine.ProcessAction(game, action);

            Assert.AreEqual(Action.ActionType.Play, game.LastActions[0].Type);
        }
    }
}
