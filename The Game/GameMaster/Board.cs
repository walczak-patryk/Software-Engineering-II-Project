using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Game.Board
{ 
    public class Board
    {
        Cell[,] cellsGrid;
        int goalAreaHeight;
        int taksAreaHeight;
        int boardWidth;
        int boardHeight;

        public Board(int boardWidth, int goalAreaHeight, int taksAreaHeight) { }
        public void GetField(SequencePosition position) { }
        public void UpdateField(Field field) { }
        public void UpdateCell(Cell cell, Position position) { }
        public Cell GetCell(Position position) { }
        public void InitializeCellGrid() { }
    }
}
