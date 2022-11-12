namespace Homestead.Shared.Tests
{
    [TestClass]
    public class GameTest
    {
        [TestMethod]
        public void CreateGame()
        {
            var game = new Game();

            Assert.AreEqual(4, game.Players.Count);
            Assert.IsTrue(game.Players[3].Name != null);
        }
    }
}