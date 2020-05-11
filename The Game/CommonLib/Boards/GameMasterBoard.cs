using System;
using System.Collections.Generic;
using System.Drawing;
using GameMaster.Cells;
using GameMaster.Fields;
using GameMaster.Positions;
using GameMaster;

namespace GameMaster.Boards
{
    public class GameMasterBoard : Board
    {

        public List<Position> piecesPositions;

        public GameMasterBoard(int boardWidth, int goalAreaHeight, int taksAreaHeight) : base(boardWidth, goalAreaHeight, taksAreaHeight)
        {
            piecesPositions = new List<Position>();
        }

        public Position PlayerMove(PlayerDTO player, Direction direction) 
        {
            Position playerPosition = player.playerPosition;
            int destinationX = playerPosition.x;
            int destinationY = playerPosition.y;
            if (direction == Direction.Right)
            {
                destinationX++;
            }
            else if (direction == Direction.Left)
            {
                destinationX--;
            }
            else if (direction == Direction.Down)
            {
                destinationY++;
            }
            else if (direction == Direction.Up)
            {
                destinationY--;
            }
            TeamColor teamColor = player.playerTeamColor;

            Position destinationPosition = new Position(destinationX, destinationY);
            switch (teamColor)
            {
                case TeamColor.Red:
                    if (0 <= destinationX && destinationX < boardWidth
                        && 0 <= destinationY && destinationY < boardHeight - goalAreaHeight)
                    {
                        if (GetCell(destinationPosition).GetPlayerGuid() == null)
                        {
                            GetCell(playerPosition).SetPlayerGuid(null);
                            GetCell(destinationPosition).SetPlayerGuid(player.playerGuid.ToString());
                            return destinationPosition;
                        }

                    }
                    break;
                case TeamColor.Blue:
                    if (0 <= destinationX && destinationX < boardWidth && goalAreaHeight <= destinationY && destinationY < boardHeight)
                    {
                        if (GetCell(destinationPosition).GetPlayerGuid() == null)
                        {
                            GetCell(playerPosition).SetPlayerGuid(null);
                            GetCell(destinationPosition).SetPlayerGuid(player.playerGuid.ToString());
                            return destinationPosition;
                        }

                    }
                    break;
            }
            return new Position(); 
        }
        public CellState TakePiece(Position position) 
        {
            Cell elem = GetCell(position);
            if (elem.GetCellState() == CellState.Piece)
            {
                elem.SetCellState(CellState.Empty);
                return CellState.Piece;
            }
            /*else if (elem.GetCellState() == CellState.Sham)
            {
                elem.SetCellState(CellState.Empty);
                return CellState.Sham;
            }*/
            return CellState.Empty;
        }
        public Position generatePiece(double chance, int maxPieces) {
            if (piecesPositions.Count >= maxPieces)
            {
                piecesPositions.Clear();
                foreach(var i in cellsGrid)
                {
                    if (i.GetCellState() == CellState.Piece /*|| i.GetCellState() == CellState.Sham*/)
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

            //if (random.NextDouble() < chance)
                //cellsGrid[pos.x, pos.y].SetCellState(CellState.Sham);
            //else
                cellsGrid[pos.x, pos.y].SetCellState(CellState.Piece);

            return pos; 
        }
        public void SetGoal(Position position) {
            GetCell(position).SetCellState(CellState.Valid);
        }
        public PlacementResult PlacePiece(Position position) 
        {
            if (GetCell(position).GetCellState() == CellState.Valid)
            {
                Cell cell = GetCell(position);
                cell.SetCellState(CellState.Goal);
                return PlacementResult.Correct;
            }
            else
                return PlacementResult.Pointless; 
        }
        public Position PlacePlayer(PlayerDTO playerDTO) 
        {
            Position pos = playerDTO.playerPosition;
            Cell cell = GetCell(pos);
            cell.SetPlayerGuid(playerDTO.playerGuid.ToString());
            return pos; 
        }
        public void CheckWinCondition(TeamColor teamColor) 
        {
            bool win;
            if(teamColor == TeamColor.Red)
            {
                for(int i = 0; i < goalAreaHeight; i++)
                {
                    for(int j = 0; j < boardWidth; j++)
                    {
                        if(GetCell(new Position(j,i)).GetCellState() == CellState.Valid)
                        {
                            win = false;
                            break;
                        }
                    }
                }
                win = true;
            }
            if (teamColor == TeamColor.Blue)
            {
                for (int i = goalAreaHeight + taskAreaHeight; i < goalAreaHeight + 2 * taskAreaHeight; i++)
                {
                    for (int j = 0; j < boardWidth; j++)
                    {
                        if (GetCell(new Position(j, i)).GetCellState() == CellState.Valid)
                        {
                            win = false;
                            break;
                        }
                    }
                }
                win = true;
            }
        }
        public List<Field> Discover(Position position)
        {
            int posX = position.x;
            int posY = position.y;
            List<int> distances = ManhattanDistance(position);
            List<Field> fields = new List<Field>();
            int iter = 0;
            for(int i = -1; i < 2; i++)
            {
                for(int j = -1; j < 2; j++)
                {
                    Position pos = new Position(posX + j, posY + i);
                    Cell cel = new Cell(distances[iter++]);
                    if (cel.GetDistance() == 0)
                        cel.SetCellState(CellState.Piece);
                    Field f = new Field() { position = pos, cell = cel };
                    fields.Add(f);
                }
            }
            return fields; 
        }
        public List<int> ManhattanDistance(Position playerPosition) {
            Console.WriteLine("DEBUG: player position - {0} , {1}", playerPosition.x, playerPosition.y);
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
                            Console.WriteLine("DEBUG: piece position - {0} , {1}", piece.x, piece.y);
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
