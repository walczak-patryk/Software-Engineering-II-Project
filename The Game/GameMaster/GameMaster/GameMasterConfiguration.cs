using System.Drawing;

namespace GameMaster
{
    public class GameMasterConfiguration
    {
        public double shamProbability = 0.5;
        public int maxTeamSize = 4;
        public int maxPieces = 3;
        public int initialPieces = 2;
        public Point[] predefinedGoalPositions = new Point[] { new Point(4, 0) };
        public int boardWidth = 7;
        public int boardTaskHeight = 7;
        public int boardGoalHeight = 3;
        public int delayDestroyPiece = 2950;
        public int delayNextPiecePlace = 3000;
        public int delayMove = 100;
        public int delayDiscover = 500;
        public int delayTest = 1000;
        public int delayPick = 100;
        public int delayPlace = 100;
        //public int delayFail = 1000;
    }
}
