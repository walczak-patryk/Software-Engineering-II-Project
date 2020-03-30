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
            Cell cell = new Cell();
            cell.SetCellState(CellState.Empty);
            Assert.AreEqual(CellState.Empty, cell.GetCellState());

        }

        [Test]
        public void WhenSettingCellGetDifferentCellState()
        {
            Cell cell = new Cell();
            cell.SetCellState(CellState.Empty);
            Assert.AreNotEqual(CellState.Piece, cell.GetCellState());

        }


        [Test]
        public void GivenPositionGetField()
        {
            GameMaster.Positions.Position position = new Position(0,0);
            Cell cell = new Cell();
            Assert.AreSame(position, cell.GetField(position));
        }

        [Test]
        public void GivenPositionGetDifferentField()
        {
            GameMaster.Positions.Position position = new Position(0,0);
            GameMaster.Positions.Position secondPosition = new Position(0,0);
            Cell cell = new Cell();
            Assert.AreNotSame(secondPosition, cell.GetField(position));

        }
    }
}
