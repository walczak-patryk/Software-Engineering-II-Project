using NUnit.Framework;
using Game.Board;

namespace Tests
{
    public class BoardIntegrationTests
    {
        [SetUp]
        public void Setup()
        {
            Board board = new Board();
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}