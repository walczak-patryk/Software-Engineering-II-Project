using GameMaster.Fields;
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
        private string playerGuid;

        public Cell()
        {
            cellState = CellState.Empty;
            distance = 0;
            playerGuid = null;
        }

        public string GetPlayerGuid()
        {
            return this.playerGuid;
        }

        public void SetPlayerGuid(string playerGuid)
        {
            this.playerGuid = playerGuid;
        }

        public CellState GetCellState()
        {
            return this.cellState;
        }

        public void SetCellState(CellState state)
        {
            this.cellState = state;
        }

        public Field GetField(Position position)
        {
            return null;
        }
    }
}
