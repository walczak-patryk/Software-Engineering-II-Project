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
        int DisplayManhattanDistance(int x, int y);
        bool FieldOccupied();
        void PlacePiece();
    }

    class Board : IBoard
    {
        public int DisplayManhattanDistance(int x, int y)
        {
            throw new NotImplementedException();
        }

        public bool FieldOccupied()
        {
            throw new NotImplementedException();
        }

        public void GeneratePiece()
        {
            throw new NotImplementedException();
        }

        public void PieceInformation()
        {
            throw new NotImplementedException();
        }

        public void PlacePiece()
        {
            throw new NotImplementedException();
        }

        public void UpdateBoard(string[] message)
        {
            throw new NotImplementedException();
        }
    }
}
