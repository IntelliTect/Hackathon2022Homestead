namespace Homestead.Shared.Tests
{
    [TestClass]
    public class CardsTest
    {
        [TestMethod]
        public void GetCard()
        {
            var card1 = Cards.GetCard(true);
            Assert.AreEqual("FloodOther", card1);
            var card2 = Cards.GetCard(true);
            Assert.AreEqual("Dog", card2);

        }
    }
}