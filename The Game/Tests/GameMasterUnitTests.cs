using GameMaster;
using GameMaster.Boards;
using GameMaster.Cells;
using GameMaster.Positions;
using NUnit.Framework;
using System.Collections.Generic;

namespace Tests
{
    public class GameMasterUnitTests
    {
        [Test]
        public void ParseNoPlayersMessageOptionsForGUI()
        {
            GameMaster.GameMaster gm = new GameMaster.GameMaster();
            GameMasterBoard gmb = new GameMasterBoard(2, 1, 2);
            gm.board = gmb;

            string expected = "o;w,2;h,4;g,1;t,2;r,0;b,0;";
            string result = gm.MessageOptionsForGUI();

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void ParseWithPlayersMessageOptionsForGUI()
        {
            GameMaster.GameMaster gm = new GameMaster.GameMaster();
            GameMasterBoard gmb = new GameMasterBoard(2, 1, 2);
            gm.board = gmb;

            string expected = "o;w,2;g,1;t,2;";
            string result = gm.MessageOptionsForGUI();

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void ParseNoPlayersMessageStateForGUI()
        {
            GameMaster.GameMaster gm = new GameMaster.GameMaster();
            GameMasterBoard gmb = new GameMasterBoard(2, 1, 2);
            Position goal = new Position(1, 0);
            Position piece = new Position(1, 1);
            gmb.SetGoal(goal);
            gmb.GetCell(piece).SetCellState(CellState.Piece);
            gm.board = gmb;

            string expected = "s;0,4,0,2,0,0,0,0;";
            string result = gm.MessageStateForGUI();

            Assert.AreEqual(CellState.Valid, gm.board.GetCell(goal).GetCellState());
            Assert.AreEqual(CellState.Piece, gm.board.GetCell(piece).GetCellState());
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void ParseWithPlayersMessageStateForGUI()
        {
            GameMaster.GameMaster gm = new GameMaster.GameMaster();
            GameMasterBoard gmb = new GameMasterBoard(2, 1, 2);
            Position goal = new Position(1, 0);
            Position piece = new Position(1, 1);

            gm.teamRedGuids = new List<string>();
            gm.teamBlueGuids = new List<string>();

            gm.teamRedGuids.Add("8");
            gm.teamBlueGuids.Add("9");

            gmb.cellsGrid[0, 0].SetPlayerGuid(gm.teamRedGuids[0]);
            gmb.cellsGrid[0, 2].SetPlayerGuid(gm.teamBlueGuids[0]);

            gmb.SetGoal(goal);
            gmb.GetCell(piece).SetCellState(CellState.Piece);
            gm.board = gmb;

            string expected = "s;7,r,8,4,0,2,7,b,9,0,0,0;";
            string result = gm.MessageStateForGUI();

            Assert.AreEqual(CellState.Valid, gm.board.GetCell(goal).GetCellState());
            Assert.AreEqual(CellState.Piece, gm.board.GetCell(piece).GetCellState());
            Assert.AreEqual(expected, result);
        }
    }

}