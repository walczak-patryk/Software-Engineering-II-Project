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

        public Cell(int maxsize)
        {
            cellState = CellState.Empty;
            distance = maxsize;
            playerGuid = null;
        }

        public Cell Copy()
        {
            Cell res = new Cell(distance);
            res.SetCellState(cellState);
            res.SetDistance(distance);
            res.SetPlayerGuid(playerGuid);
            return res;
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

        public int GetDistance()
        {
            return distance;
        }

        public void SetDistance(int dist)
        {
            distance = dist;
        }
    }
}
