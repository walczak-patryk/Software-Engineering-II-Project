using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace GameMaster
{


    interface IBoard
    {
        void UpdateBoard(string[] message);
        void PieceInformation();
        void GeneratePiece();
        int DisplayManhattanDistance(Point point);
        bool FieldOccupied();
        void PlacePiece();
    }

    class Board
    {
    }
}
