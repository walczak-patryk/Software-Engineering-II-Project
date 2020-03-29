using GameMaster.Positions;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameMaster.Cells
{
    public class Cell
    {
        private CellState cellState;
        private int distance;
        private string playGuids;

        public CellState GetCellState()
        {
            return this.cellState;
        }

        public void SetCellState(CellState state)
        {
            this.cellState = state;
        }

        public Position GetField(Position position)
        {
            return position;
        }
    }
}
