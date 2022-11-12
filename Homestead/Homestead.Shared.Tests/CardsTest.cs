namespace Homestead.Shared.Tests
{
    [TestClass]
    public class CardsTest
    {
        [TestMethod]
        public void GetCard()
        {
            var card1 = Cards.GetCard(true);
            Assert.AreEqual(Cards.BadNeighbor, card1);
            var card2 = Cards.GetCard(true);
            Assert.AreEqual(Cards.FloodOther, card2);
        }

        [TestMethod]
        public void GetCardInfo()
        {
            var info = Cards.GetCardInfo(Cards.FloodAll);
            Assert.AreEqual(CardInfo.CardImpact.All, info.Impact);
            Assert.AreEqual(CardInfo.CardSuit.Disaster, info.Suit);
            Assert.AreEqual("flood.png", info.ImageFilename);
            Assert.AreEqual("/Assets/Images/cards/flood.png", info.ImageUrl);
        }
    }
}