using GameMaster;
using GameMaster.Boards;
using GameMaster.Cells;
using GameMaster.Fields;
using GameMaster.Positions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Windows.Navigation;

namespace Tests
{
    public class BoardUnitTests
    {
        [SetUp]
        public void Setup()
        {
           // Board board = new Board();
        }

        [Test]
        public void CreateBoard()
        {
            Board board = new Board(5, 5, 3);

            Assert.AreEqual(5, board.boardWidth);
            Assert.AreEqual(5, board.goalAreaHeight);
            Assert.AreEqual(3, board.taskAreaHeight);
        }

        [Test]
        public void GivenPositionUpdateCell()
        {
            Board board = new Board(5, 5, 3);
            Position position = new Position(1, 1);
            Cell cell = new Cell(0);
            cell.SetCellState(CellState.Goal);
            cell.SetDistance(1);
            board.UpdateCell(cell, position);

            Assert.AreEqual(CellState.Goal, board.GetCell(position).GetCellState());
            Assert.AreEqual(1, board.GetCell(position).GetDistance());
        }

        [Test]
        public void InitializeCellGridCorrectly()
        {
            Board board = new Board(2, 2, 2);
            for (int i = 0; i < 2; i++)
                for (int j = 0; j < 6; j++)
                    Assert.IsNotNull(board.cellsGrid[i, j]);
        }

        [Test]
        public void PlayerMoveOnGameMasterBoard()
        {
            GameMasterBoard gmboard = new GameMasterBoard(3, 3, 3);
            PlayerDTO player = new PlayerDTO();
            Position oldPosition = new Position(1, 1);
            player.playerPosition = oldPosition;

            Assert.AreNotEqual(oldPosition, gmboard.PlayerMove(player, Direction.Down));
            Assert.AreNotEqual(oldPosition, gmboard.PlayerMove(player, Direction.Left));
            Assert.AreNotEqual(oldPosition, gmboard.PlayerMove(player, Direction.Right));
            Assert.AreNotEqual(oldPosition, gmboard.PlayerMove(player, Direction.Up));
        }

        [Test]
        public void TakePieceOrShamOnGameMaster()
        {
            GameMasterBoard gmboard = new GameMasterBoard(3, 3, 3);
            gmboard.cellsGrid[1, 1].SetCellState(CellState.Piece);
            gmboard.cellsGrid[1, 2].SetCellState(CellState.Sham);
            Position piece = new Position(1, 1);
            Position sham = new Position(1, 2);

            Assert.AreNotEqual(CellState.Piece, gmboard.TakePiece(piece));
            Assert.AreNotEqual(CellState.Sham, gmboard.TakePiece(piece));
            Assert.AreNotEqual(CellState.Sham, gmboard.TakePiece(sham));
            Assert.AreNotEqual(CellState.Piece, gmboard.TakePiece(sham));
        }

        [Test]
        public void TryToTakePieceOnGameMaster()
        {
            GameMasterBoard gmboard = new GameMasterBoard(3, 3, 3);
            gmboard.cellsGrid[1, 1].SetCellState(CellState.Empty);
            Position position = new Position(1, 1);

            Assert.AreNotEqual(CellState.Piece, gmboard.TakePiece(position));
            Assert.AreNotEqual(CellState.Sham, gmboard.TakePiece(position));
        }

        [Test]
        public void GenerateOneRandomPiece()
        {
            GameMasterBoard gmboard = new GameMasterBoard(3, 3, 3);
            Position position = gmboard.generatePiece(0.25, 2);

            int pieces = 0;
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 9; j++)
                    if (gmboard.cellsGrid[i, j].GetCellState() == CellState.Piece || gmboard.cellsGrid[i, j].GetCellState() == CellState.Sham)
                        pieces++;
            Assert.AreEqual(1, pieces);
        }

        [Test]
        public void GenerateMoreThanMaxPieces()
        {
            GameMasterBoard gmboard = new GameMasterBoard(3, 3, 3);
            gmboard.generatePiece(0.25, 2);
            gmboard.generatePiece(0.25, 2);

            int first = 0;
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 9; j++)
                    if (gmboard.cellsGrid[i, j].GetCellState() == CellState.Piece || gmboard.cellsGrid[i, j].GetCellState() == CellState.Sham)
                        first++;

            gmboard.generatePiece(0.25, 2);

            int second = 0;
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 9; j++)
                    if (gmboard.cellsGrid[i, j].GetCellState() == CellState.Piece || gmboard.cellsGrid[i, j].GetCellState() == CellState.Sham)
                        second++;

            Assert.AreEqual(2, first);
            Assert.AreEqual(1, second);
        }

        [Test]
        public void SetGoalOnGameMasterBoard()
        {
            GameMasterBoard gmboard = new GameMasterBoard(2, 2, 2);
            Position position = new Position(1, 1);
            gmboard.SetGoal(position);

            Assert.AreEqual(CellState.Goal, gmboard.GetCell(position).GetCellState());
        }

        [Test]
        public void ManhattanDistanceWhenNoPieceOnBoard()
        {
            GameMasterBoard gmboard = new GameMasterBoard(2, 2, 2);
            Position player1 = new Position(1, 1);
            Position player2 = new Position(2, 3);
            List<int> result1 = gmboard.ManhattanDistance(player1);
            List<int> result2 = gmboard.ManhattanDistance(player2);

            int max = System.Math.Max(gmboard.boardWidth, gmboard.boardHeight + gmboard.taskAreaHeight);
            bool no1 = true;
            bool no2 = true;

            foreach(var dist in result1)
                if (dist != max)
                    no1 = false;

            foreach (var dist in result2)
                if (dist != max)
                    no2 = false;

            Assert.IsTrue(no1);
            Assert.IsTrue(no2);
        }

        [Test]
        public void ManhattanDistanceWhenOnePieceOnBoard()
        {
            GameMasterBoard gmboard = new GameMasterBoard(2, 2, 2);
            Position player1 = new Position(1, 1);
            Position player2 = new Position(2, 3);

            Position piece = gmboard.generatePiece(0.25, 2);

            List<int> result1 = gmboard.ManhattanDistance(player1);
            List<int> result2 = gmboard.ManhattanDistance(player2);

            int max = System.Math.Max(gmboard.boardWidth, gmboard.boardHeight + gmboard.taskAreaHeight);
            bool no1 = true;
            bool no2 = true;

            foreach (var dist in result1)
                if (dist != max)
                    no1 = false;

            foreach (var dist in result2)
                if (dist != max)
                    no2 = false;

            Assert.IsFalse(no1);
            Assert.IsFalse(no2);
        }
    }
}