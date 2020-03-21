using NUnit.Framework;
using Game.GameMaster;

namespace Tests
{
    public class GameMasterIntegrationTests
    {
        [SetUp]
        public void Setup()
        {
            GameMaster gameMaster = new GameMaster();
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}