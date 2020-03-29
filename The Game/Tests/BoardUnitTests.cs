using Game.Board;
using NUnit.Framework;

namespace Tests
{
    public class BoardUnitTests
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