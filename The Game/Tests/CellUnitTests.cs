using System;
using System.Collections.Generic;
using System.Text;
using GameMaster.Cells;
using NUnit.Framework;
using GameMaster.Positions;

namespace Tests
{
    public class CellUnitTests
    {

        [Test]
        public void WhenSettingCellGetCellState()
        {
            Cell cell = new Cell(0);
            cell.SetCellState(CellState.Empty);
            Assert.AreEqual(CellState.Empty, cell.GetCellState());

        }

        [Test]
        public void WhenSettingCellGetDifferentCellState()
        {
            Cell cell = new Cell(0);
            cell.SetCellState(CellState.Empty);
            Assert.AreNotEqual(CellState.Piece, cell.GetCellState());

        }

        [Test]
        public void WhenSettingCellPlayerGuidGetCellPlayerGuid()
        {
            Cell cell = new Cell(0);
            cell.SetPlayerGuid("test");
            Assert.AreEqual("test", cell.GetPlayerGuid());

        }

        [Test]
        public void WhenSettingCellPlayerGuidGetDifferentCellPlayerGuid()
        {
            Cell cell = new Cell(0);
            cell.SetPlayerGuid("cell");
            Assert.AreNotEqual("test", cell.GetPlayerGuid());

        }

        [Test]
        public void WhenSettingCellDistanceGetCellDistance()
        {
            Cell cell = new Cell(0);
            cell.SetDistance(1);
            Assert.AreEqual(1, cell.GetDistance());

        }

        [Test]
        public void WhenSettingCellDistanceGetDifferentCellDistance()
        {
            Cell cell = new Cell(0);
            cell.SetDistance(1);
            Assert.AreNotEqual(0, cell.GetDistance());

        }

        [Test]
        public void GivenPositionGetDifferentField()
        {
            GameMaster.Positions.Position position = new Position(0,0);
            GameMaster.Positions.Position secondPosition = new Position(0,0);
            Cell cell = new Cell(0);
            Assert.AreNotSame(secondPosition, cell.GetField(position));

        }
    }
}
