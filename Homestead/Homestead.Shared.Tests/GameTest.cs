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

            Assert.HasCount(4, game.Players);
            Assert.IsNotNull(game.Players[3].Name);
        }
    }
}