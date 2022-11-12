using Homestead.Server.SignalR;

namespace Homestead.Shared.Tests
{
    [TestClass]
    public class GameLookupTest
    {
        GameLookup _lookup { get; set; }
        GameLookupTest(GameLookup lookup)
        {
            _lookup = lookup;
        }
        [TestInitialize]

        [TestMethod]
        public void addGame()
        {
            var game = new Game();
            _lookup.AddGame(game);

        }

        [TestMethod]
        public void GetGame()
        {

        }
    }
}