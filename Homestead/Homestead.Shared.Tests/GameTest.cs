namespace Homestead.Shared.Tests
{
    [TestClass]
    public class GameTest
    {
        [TestMethod]
        public void CreateGame()
        {
            GameEngineWithActions engine = new();
            Game game = engine.Start("test");

            Assert.AreEqual(4, game.Players.Count);
            Assert.IsTrue(game.Players[3].Name != null);
        }
    }
}