using System;
using System.Collections.Generic;
using System.Text;

namespace GameMaster
{
    public class GameMasterConfiguration
    {
        public double shamProbability;
        public int maxTeamSize;
        public int maxPieces;
        public int initialPieces;
        public Point[] predefinedGoalPositions;
        public int boardWidth;
        public int boardTaskHeight;
        public int boardGoalHeight;
        public int delayDestroyPiece;
        public int delayNextPiecePlace;
        public int delayMove;
        public int delayDiscover;
        public int delayTest;
        public int delayPick;
        public int delayPlace;
    }
}
