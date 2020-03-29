using System;
using System.Collections.Generic;
using System.Drawing;
using GameMaster.Cells;
using GameMaster.Fields;
using GameMaster.Positions;

namespace GameMaster.Boards
{
    public class GameMasterBoard : Board
    {

        ISet<Position> piecesPositions;

        public GameMasterBoard(int boardWidth, int goalAreaHeight, int taksAreaHeight) : base(boardWidth, goalAreaHeight, taksAreaHeight)
        { }

        public Position PlayerMove(PlayerDTO player, Direction direction) { return new Position(); }
        public CellState TakePiece(Position position) { return new CellState(); }
        public Position generatePiece(double chance) { return new Position(); }
        public void SetGoal(Position position) { }
        public PlacementResult PlacePiece(Position position) { return PlacementResult.Correct; }
        public Position PlacePlayer(PlayerDTO playerDTO) { return new Position(); }
        public void CheckWinCondition(TeamColor teamColor) { }
        public List<Field> Discover(Position position) { return new List<Field>(); }
        public int ManhattanDistanceTwoPoints(Point pointA, Point pointB) { return 1; }
    }

    public enum PlacementResult{
        Correct,
        Pointless
    }
}
