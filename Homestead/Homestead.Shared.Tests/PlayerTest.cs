namespace Homestead.Shared.Tests
{
    [TestClass]
    public class PlayerTest
    {
        [TestMethod]
        public void Suits()
        {
            var player = new Player(1);
            player.Board.Add(Cards.Saw);
            player.Board.Add(Cards.Well);
            player.Board.Add(Cards.Hammer);
            player.Board.Add(Cards.Shovel);
            player.Board.Add(Cards.Stove);
            player.Board.Add(Cards.Livestock);

            Assert.AreEqual(2, player.BoardBySuit(CardInfo.CardSuit.Garden).Count());
            Assert.AreEqual(1, player.BoardBySuit(CardInfo.CardSuit.LiveStock).Count());
            Assert.AreEqual(3, player.BoardBySuit(CardInfo.CardSuit.House).Count());
            Assert.AreEqual(0, player.BoardBySuit(CardInfo.CardSuit.Action).Count());
        }
    }
}