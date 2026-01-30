using Homestead.Server.SignalR;

namespace Homestead.Shared.Tests
{
    [TestClass]
    public class GameLookupTest
    {
        GameLookup _lookup { get; set; } = new GameLookup();
        
        [TestInitialize]
        public void Initialize()
        {
            _lookup = new GameLookup();
        }

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