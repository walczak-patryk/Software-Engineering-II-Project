using System;
using System.Collections.Generic;
using System.Text;

namespace GameMaster
{
    class Player
    {
        public int id;
        public string playerName;
        public Team team;
        public bool isLeader;
        public Position position;
        public Piece piece;
        public Board board;
        public ActionType lastAction;
        private string guid;
        private PlayerState state;

        public Player(int Id, string name, Team Team, bool IsLeader)
        {
            this.id = Id;
            this.team = Team;
            this.isLeader = IsLeader;
            this.position = new Position(-1, -1);
            this.piece = null;
            this.board = new Board();
        }

        private void Move(int x, int y)
        {
            this.position = new Position(x, y);
        }

        private void TakePiece(Piece piece)
        {
            if(this.piece == null)
                this.piece = piece;
        }

        private void TestPiece()
        {

        }

        private void PlacePiece()
        {

        }
    }

    class PlayerDto
    {
        public Guid playerGiud;
        public Position playerPosition;
        public TeamRole playerTeamRole;
        public TeamColor playerTeamColor;
        public ActionType playerAction;
    }

    public enum PlayerState
    {
        Initializing,
        Active,
        Completed
    }

    public enum ActionType
    {
        Move,
        Pickup,
        Test,
        Place,
        Destroy,
        Send
    }
}
