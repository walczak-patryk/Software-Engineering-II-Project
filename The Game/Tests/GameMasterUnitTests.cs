using Game.GameMaster;
using NUnit.Framework;

namespace Tests
{
    public class GameMasterUnitTests
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