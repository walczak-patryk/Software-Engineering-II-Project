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

        public List<Position> piecesPositions;

        public GameMasterBoard(int boardWidth, int goalAreaHeight, int taksAreaHeight) : base(boardWidth, goalAreaHeight, taksAreaHeight)
        {
            piecesPositions = new List<Position>();
        }

        public Position PlayerMove(PlayerDTO player, Direction direction) { return new Position(); }
        public CellState TakePiece(Position position) { return new CellState(); }
        public Position generatePiece(double chance, int maxPieces) {
            if (piecesPositions.Count >= maxPieces)
            {
                piecesPositions.Clear();
                foreach(var i in cellsGrid)
                {
                    if (i.GetCellState() == CellState.Piece || i.GetCellState() == CellState.Sham)
                        i.SetCellState(CellState.Empty);
                }
            }

            Random random = new Random();
            int x = random.Next() % boardWidth;
            int y = random.Next() % taskAreaHeight;
            Position pos = new Position();
            pos.x = x;
            pos.y = y + goalAreaHeight;
            while (piecesPositions.Find(p => p.x == pos.x && p.y == pos.y) != null)
            {
                pos.x = random.Next() % boardWidth;
                pos.y = random.Next() % taskAreaHeight + goalAreaHeight;
            }
            piecesPositions.Add(pos);

            if (random.NextDouble() < chance)
                cellsGrid[pos.x, pos.y].SetCellState(CellState.Sham);
            else
                cellsGrid[pos.x, pos.y].SetCellState(CellState.Piece);

            return pos; 
        }
        public void SetGoal(Position position) {
            GetCell(position).SetCellState(CellState.Valid);
        }
        public PlacementResult PlacePiece(Position position) { return PlacementResult.Correct; }
        public Position PlacePlayer(PlayerDTO playerDTO) { return new Position(); }
        public void CheckWinCondition(TeamColor teamColor) { }
        public List<Field> Discover(Position position) { return new List<Field>(); }
        public List<int> ManhattanDistance(Position playerPosition) {
            List<int> list = new List<int>();
            for (int j = -1; j <= 1; j++)
            {
                for (int i = -1; i <= 1; i++)
                {
                    if (playerPosition.x + i < 0 || playerPosition.x + i >= boardWidth || playerPosition.y + j < goalAreaHeight || playerPosition.y + j >= goalAreaHeight + taskAreaHeight)
                        list.Add(Math.Max(boardWidth, boardHeight + taskAreaHeight));
                    else
                    {
                        int distance = Math.Max(boardWidth, boardHeight);
                        foreach (var piece in piecesPositions) {
                            if (distance > Math.Abs(playerPosition.x + i - piece.x) + Math.Abs(playerPosition.y + j - piece.y))
                                distance = Math.Abs(playerPosition.x + i - piece.x) + Math.Abs(playerPosition.y + j - piece.y);
                        }
                        list.Add(distance);
                    }
                }
            }
            return list;
        }
    }

    public enum PlacementResult{
        Correct,
        Pointless
    }
}
