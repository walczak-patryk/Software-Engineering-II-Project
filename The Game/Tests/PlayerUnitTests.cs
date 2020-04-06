using System;
using System.Collections.Generic;
using System.Text;
using GameMaster;
using GameMaster.Boards;
using GameMaster.Cells;
using GameMaster.Positions;
using NUnit.Framework;
namespace Tests
{
    public class PlayerUnitTests
    {

        [Test]
        public void GivenPieceInGoalAreaPlacePieceOnValid()
        {
            Team team = new Team();
            team.SetColor(TeamColor.Red);
            Position position = new Position(1, 1);
            Player player = new Player(1,team,false);

            GameMasterBoard masterBoard = new GameMasterBoard(10, 5, 10);
            player.board = masterBoard;

            player.piece = true;
            player.pieceIsSham = false;
            player.position = position;

            
            Cell cell = new Cell(0);
            cell.SetCellState(CellState.Valid);
            player.board.UpdateCell(cell,position);

            player.PlacePiece();
            Assert.AreEqual(CellState.Goal, player.board.GetCell(position).GetCellState());

        }

        [Test]
        public void GivenShamInGoalAreaPlacePieceOnValid()
        {
            Team team = new Team();
            team.SetColor(TeamColor.Red);
            Position position = new Position(1, 1);
            Player player = new Player(1, team, false);

            GameMasterBoard masterBoard = new GameMasterBoard(10, 5, 10);
            player.board = masterBoard;
            player.piece = true;
            player.pieceIsSham = true;
            player.position = position;


            Cell cell = new Cell(0);
            cell.SetCellState(CellState.Valid);
            player.board.UpdateCell(cell, position);

            player.PlacePiece();
            Assert.AreEqual(CellState.Valid, player.board.GetCell(position).GetCellState());

        }

        [Test]
        public void GivenPieceInAreaPlacePieceOnNonValid()
        {
            Team team = new Team();
            team.SetColor(TeamColor.Red);
            Position position = new Position(1, 1);
            Player player = new Player(1, team, false);

            GameMasterBoard masterBoard = new GameMasterBoard(10, 5, 10);
            player.board = masterBoard;
            player.piece = true;
            player.pieceIsSham = false;
            player.position = position;


            Cell cell = new Cell(0);
            cell.SetCellState(CellState.Unknown);
            player.board.UpdateCell(cell, position);

            player.PlacePiece();
            Assert.AreEqual(CellState.Unknown, player.board.GetCell(position).GetCellState());

        }

        [Test]
        public void GivenPieceInTaskPlacePiece()
        {
            Team team = new Team();
            team.SetColor(TeamColor.Red);
            Position position = new Position(8, 8) ;
            Player player = new Player(1, team, false);

            GameMasterBoard masterBoard = new GameMasterBoard(10, 5, 10);
            player.board = masterBoard;
            player.piece = true;
            player.pieceIsSham = false;
            player.position = position;


            Cell cell = new Cell(0);
            cell.SetCellState(CellState.Unknown);
            player.board.UpdateCell(cell, position);

            player.PlacePiece();
            Assert.AreEqual(CellState.Unknown, player.board.GetCell(position).GetCellState());

        }

        [Test]
        public void GivenPieceTakePiece()
        {

            Position position = new Position(8, 8);
            Player player = new Player(1, new Team(), false);

            GameMasterBoard masterBoard = new GameMasterBoard(10, 5, 10);
            player.board = masterBoard;
            player.piece = false;
            player.pieceIsSham = false;
            player.position = position;


            Cell cell = new Cell(0);
            cell.SetCellState(CellState.Piece);
            player.board.UpdateCell(cell, position);

            player.TakePiece();
            Assert.AreEqual(true, player.piece);
            Assert.AreEqual(false, player.pieceIsSham);
            Assert.AreEqual(CellState.Empty, player.board.GetCell(position).GetCellState());

        }

        [Test]
        public void GivenShamTakePiece()
        {

            Position position = new Position(8, 8);
            Player player = new Player(1, new Team(), false);

            GameMasterBoard masterBoard = new GameMasterBoard(10, 5, 10);
            player.board = masterBoard;
            player.piece = false;
            player.pieceIsSham = false;
            player.position = position;


            Cell cell = new Cell(0);
            cell.SetCellState(CellState.Sham);
            player.board.UpdateCell(cell, position);

            player.TakePiece();
            Assert.AreEqual(true, player.piece);
            Assert.AreEqual(true, player.pieceIsSham);
            Assert.AreEqual(CellState.Empty, player.board.GetCell(position).GetCellState());

        }


        [Test]
        public void GivenNotPieceTakePiece()
        {

            Position position = new Position(8, 8);
            Player player = new Player(1, new Team(), false);

            GameMasterBoard masterBoard = new GameMasterBoard(10, 5, 10);
            player.board = masterBoard;
            player.piece = false;
            player.pieceIsSham = false;
            player.position = position;


            Cell cell = new Cell(0);
            cell.SetCellState(CellState.Unknown);
            player.board.UpdateCell(cell, position);

            player.TakePiece();
            Assert.AreEqual(false, player.piece);
            Assert.AreEqual(false, player.pieceIsSham);
            Assert.AreEqual(CellState.Unknown, player.board.GetCell(position).GetCellState());

        }


        //[Test]
        //public void GivenPlayerMove()
        //{
        //    Team team = new Team();
        //    team.color = TeamColor.Red;
        //    Position position = new Position(7, 6);
        //    Player player = new Player(1, "Test", team, false);

        //    GameMasterBoard masterBoard = new GameMasterBoard(10, 5, 10);
        //    player.board = masterBoard;

        //    player.position = position;

        //    Direction x = Direction.Right;
        //    Direction y = Direction.Down;

        //    player.Move(x, y);

        //    Assert.AreEqual(8, player.position.x);
        //    Assert.AreEqual(7, player.position.y);

        //}




    }


}
