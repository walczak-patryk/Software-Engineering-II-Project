using GameMaster.Boards;
using GameMaster.Positions;

namespace GameMaster
{
    public class Player
    {
        public int id;
        public string playerName;
        public Team team;
        public bool isLeader;
        public Position position;
        public bool piece;
        public Board board;
        public ActionType lastAction;
        private string guid;
        private PlayerState state;

        public Player(int Id, string name, Team Team, bool IsLeader)
        {
            this.id = Id;
            this.team = Team;
            this.isLeader = IsLeader;
            this.position = new Position(0,0);
            this.piece = false;
            this.board = new Board(0,0,0);
        }

        private void Move(int x, int y)
        {
            this.position = new Position(x,y);
        }

        private void TakePiece(bool piece)
        {
            this.piece = piece;
        }

        private void TestPiece()
        {

        }

        private void PlacePiece()
        {

        }
    }

    public class PlayerDTO
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
