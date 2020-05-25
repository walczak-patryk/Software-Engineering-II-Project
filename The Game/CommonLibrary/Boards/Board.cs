using GameMaster.Cells;
using GameMaster.Fields;
using GameMaster.Positions;
using System;

namespace GameMaster.Boards
{
    public class Board
    {
        public Cell[,] cellsGrid;
        public int goalAreaHeight;
        public int taskAreaHeight;
        public int boardWidth;
        public int boardHeight;

        public Board(int boardWidth, int goalAreaHeight, int taskAreaHeight) {
            this.boardWidth = boardWidth;
            this.goalAreaHeight = goalAreaHeight;
            this.taskAreaHeight = taskAreaHeight;
            boardHeight = 2 * goalAreaHeight + taskAreaHeight;
            cellsGrid = new Cell[boardWidth, boardHeight];

            InitializeCellGrid();
        }
        public Field GetField(Position position) 
        {
            return cellsGrid[position.x, position.y].GetField(position);
        
        }
        public void UpdateField(Field field) { }
        public void UpdateCell(Cell cell, Position position) {
            cellsGrid[position.x, position.y] = cell;
        }
        public Cell GetCell(Position position) {
            return cellsGrid[position.x, position.y]; 
        }
        public void InitializeCellGrid() {
            for (int i = 0; i < boardWidth; i++)
            {
                for (int j = 0; j < boardHeight; j++)
                    cellsGrid[i, j] = new Cell(Math.Max(boardWidth, goalAreaHeight + taskAreaHeight));
            }
        }
    }
}
