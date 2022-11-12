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
            Assert.AreEqual(PlayerAction.ActionType.DrawFromDeck, game.AvailableActions[0].Type);
            Assert.AreEqual(1, game.AvailableActions[0].PlayerNumber);
        }


        [TestMethod]
        public void DrawFromDeckTest()
        {
            GameEngine engine = new();
            Game game = engine.Start();

            game = engine.ProcessAction(game, new PlayerAction(PlayerAction.ActionType.DrawFromDeck, game.ActivePlayer));
            Assert.IsTrue(game.Players[game.ActivePlayer-1].Hand.Any());
            Assert.IsFalse(game.AvailableActions.Any(a => a.Type is PlayerAction.ActionType.DrawFromDeck));
            Assert.IsFalse(game.AvailableActions.Any(a => a.Type is PlayerAction.ActionType.DrawFromDiscard));
            // Test to make sure available actions are exactly:
            //  Play
            //  End Turn
            //  Discard
        }

        [TestMethod]
        [Ignore]
        public void DiscardCardTest()
        {
            GameEngine engine = new();
            Game game = engine.Start();

            PlayerAction action = new(PlayerAction.ActionType.Discard, game.ActivePlayer, Cards.Well);

            game.Players[game.ActivePlayer].Hand.Add(Cards.Well);

            Assert.IsTrue(game.Players[game.ActivePlayer].Hand.Any());
            game = engine.ProcessAction(game, action);
            Assert.IsFalse(game.Players[game.ActivePlayer].Hand.Any());
        }

        // Test for discarding a card not in your hand
        // Test for trying to discard with no cards in hand

        [TestMethod]
        [Ignore]
        public void DrawFromDiscardTest()
        {
            GameEngine engine = new();
            Game game = engine.Start();

            PlayerAction action = new(PlayerAction.ActionType.DrawFromDiscard, game.ActivePlayer);
            game.DiscardPile.Add(Cards.Well);

            game = engine.ProcessAction(game, action);
            Assert.IsFalse(game.DiscardPile.Any());
            Assert.IsTrue(game.Players[game.ActivePlayer].Hand.Any());
            Assert.IsTrue(game.Players[game.ActivePlayer].Hand[0] is Cards.Well);

            Assert.IsFalse(game.AvailableActions.Any(a => a.Type is PlayerAction.ActionType.DrawFromDeck));
            Assert.IsFalse(game.AvailableActions.Any(a => a.Type is PlayerAction.ActionType.DrawFromDiscard));
            // Test to make sure available actions are exactly:
            //  Play
            //  End Turn
            //  Discard
        }

        // Attempt to draw from discard when no card exists in discard pile

        [TestMethod]
        [Ignore]
        public void DrawFromDiscardWhenMultipleOfSameTypeExist()
        {
            GameEngine engine = new();
            Game game = engine.Start();

            PlayerAction action = new(PlayerAction.ActionType.DrawFromDiscard, game.ActivePlayer);
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

            game.Players[game.ActivePlayer-1].Hand.Add(Cards.Well);
            PlayerAction action = new(PlayerAction.ActionType.Play, game.ActivePlayer, Cards.Well);

            game = engine.ProcessAction(game, action);

            Assert.AreEqual(PlayerAction.ActionType.Play, game.LastActions[0].Type);
            Assert.AreEqual(Cards.Well, game.LastActions[0].PlayerCard);
        }

        [TestMethod]
        public void PlayCardWithoutCardInHandThrowsKeyNotFound()
        {
            GameEngine engine = new();
            Game game = engine.Start();

            PlayerAction action = new(PlayerAction.ActionType.Play, game.ActivePlayer);
            action.PlayerCard = Cards.Well;

            Assert.ThrowsException<KeyNotFoundException>(() => engine.ProcessAction(game, action));
        }

        // Active Player reverts to 1 after 4


        [TestMethod]
        [Ignore]
        public void PlayCardGiveCard()
        {
            GameEngine engine = new();
            Game game = engine.Start();
            game.Players.Add(new Player());

            game.Players[game.ActivePlayer].Hand.Add(Cards.Well);
            game.Players[game.ActivePlayer].Hand.Add(Cards.GoodNeighbor);
            PlayerAction action = new(PlayerAction.ActionType.Play, game.ActivePlayer);
            action.TargetPlayer = 2;
            action.PlayerCard = Cards.GoodNeighbor;
            action.TargetCard = Cards.Well;
            //action.PlayerCard = "Give";

            Assert.AreEqual(2, game.Players[game.ActivePlayer].Hand.Count);
            Assert.AreEqual(0, game.Players[2].Hand.Count);

            game = engine.ProcessAction(game, action);

            Assert.AreEqual(0, game.Players[game.ActivePlayer].Hand.Count);
            Assert.AreEqual(1, game.Players[2].Hand.Count);
        }

        [TestMethod]
        [Ignore]
        public void PlayCardGiveCardWithoutAdditionalCardInHand()
        {
            GameEngine engine = new();
            Game game = engine.Start();
            game.Players.Add(new Player());

            game.Players[game.ActivePlayer].Hand.Add(Cards.GoodNeighbor);
            PlayerAction action = new(PlayerAction.ActionType.Play, game.ActivePlayer);
            action.PlayerCard = Cards.GoodNeighbor;

            Assert.AreEqual(1, game.Players[game.ActivePlayer].Hand.Count);
            Assert.AreEqual(0, game.Players[2].Hand.Count);

            Assert.ThrowsException<NullReferenceException>(() => engine.ProcessAction(game, action));

            Assert.AreEqual(0, game.Players[game.ActivePlayer].Hand.Count);
            Assert.AreEqual(0, game.Players[2].Hand.Count);
        }

        [TestMethod]
        [Ignore]
        public void PlayCardStealCard()
        {
            GameEngine engine = new();
            Game game = engine.Start();
            game.Players.Add(new Player());

            game.Players[game.ActivePlayer].Hand.Add(Cards.BadNeighbor);
            game.Players[2].Hand.Add(Cards.Well);
            PlayerAction action = new(PlayerAction.ActionType.Play, game.ActivePlayer);
            action.TargetPlayer = 2;
            action.PlayerCard = Cards.BadNeighbor;
            action.TargetCard = Cards.Well;
            //action.PlayerCard = "Give";

            Assert.AreEqual(1, game.Players[game.ActivePlayer].Hand.Count);
            Assert.AreEqual(1, game.Players[2].Hand.Count);

            game = engine.ProcessAction(game, action);

            Assert.AreEqual(1, game.Players[game.ActivePlayer].Hand.Count);
            Assert.AreEqual(0, game.Players[2].Hand.Count);
        }

        // Player cannot draw two cards from deck
        // Player cannot draw two cards from discard pile
        // Player cannot draw two cards, one from deck then one from discard
        // Player cannot draw from discard if it's a disaster
        // After end turn, next player must draw from deck only if nothing exists in discard

        [TestMethod]
        [Ignore]
        public void CannotEndTurn1()
        {
            GameEngine engine = new();
            Game game = engine.Start();

            game.Players[game.ActivePlayer].Hand.Add(Cards.Well);
            game.Players[game.ActivePlayer].Hand.Add(Cards.Well);
            game.Players[game.ActivePlayer].Hand.Add(Cards.Well);

            PlayerAction action = new(PlayerAction.ActionType.Discard, game.ActivePlayer, Cards.Well);

            game = engine.ProcessAction(game, action);
            Assert.IsFalse(game.AvailableActions.Any(a => a.Type is PlayerAction.ActionType.EndTurn));
        }

        [TestMethod]
        [Ignore]
        public void Wolves()
        {
            GameEngine engine = new();
            Game game = engine.Start();
            game.AvailableActions.Clear();

            game.Players[game.ActivePlayer].Hand.Add(Cards.WolfAll);

            PlayerAction action = new(PlayerAction.ActionType.Play, game.ActivePlayer, Cards.WolfAll);

            game = engine.ProcessAction(game, action);
            Assert.AreEqual(4, game.AvailableActions.Count);
            Assert.IsTrue(game.AvailableActions.All(a => a.Type is PlayerAction.ActionType.Discard));
        }

        [TestMethod]
        [Ignore]
        public void GameTest()
        {
            GameEngine engine = new();
            Game game = engine.Start();

            game = engine.ProcessAction(game, game.AvailableActions.First());
            game = engine.ProcessAction(game, game.AvailableActions.First());
            game = engine.ProcessAction(game, game.AvailableActions.First());
            game = engine.ProcessAction(game, game.AvailableActions.First());
            game = engine.ProcessAction(game, game.AvailableActions.First());
            game = engine.ProcessAction(game, game.AvailableActions.First());
            Assert.IsTrue(game.Players[game.ActivePlayer - 1].Hand.Any());
            Assert.IsFalse(game.AvailableActions.Any(a => a.Type is PlayerAction.ActionType.DrawFromDeck));
            Assert.IsFalse(game.AvailableActions.Any(a => a.Type is PlayerAction.ActionType.DrawFromDiscard));
            // Test to make sure available actions are exactly:
            //  Play
            //  End Turn
            //  Discard
        }
    }
}
