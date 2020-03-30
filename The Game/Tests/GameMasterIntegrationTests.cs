using NUnit.Framework;
using GameMaster;

namespace Tests
{
    public class GameMasterIntegrationTests
    {
        [SetUp]
        public void Setup()
        {
            GameMaster.GameMaster gameMaster = new GameMaster.GameMaster();
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}