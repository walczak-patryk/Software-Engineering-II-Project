using GameMaster;
using NUnit.Framework;

namespace Tests
{
    public class GameMasterUnitTests
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