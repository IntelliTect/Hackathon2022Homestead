using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homestead.Shared.Tests
{
    public class GameEngineTest
    {
        [TestMethod]
        public void DrawCardTest()
        {
            GameEngine engine = new GameEngine();
            engine.DrawCard();
        }
    }
}
