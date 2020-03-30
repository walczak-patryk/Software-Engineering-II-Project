using GameMaster.Boards;
using GameMaster.Cells;
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
        public bool pieceIsSham;
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
            this.pieceIsSham = false;
            this.board = new Board(0,0,0);
        }

        private void Move(Direction x, Direction y)
        {
            int destinationX = position.x + (int)x;
            int destinationY = position.y + (int)y;
            switch (team.color)
            {
                case TeamColor.Red:
                    if (0 <= destinationX && destinationX < board.boardWidth
                        && 0 <= destinationY && destinationY < board.boardHeight - board.goalAreaHeight)
                    {
                        if (board.GetCell(new Position(destinationX, destinationY)).GetPlayerGuid() == null)
                        {
                            position.ChangePosition(x);
                            position.ChangePosition(y);
                        }

                    }
                    break;
                case TeamColor.Blue:
                    if (0 <= destinationX && destinationX < board.boardWidth
    && board.goalAreaHeight <= destinationY && destinationY < board.boardHeight)
                    {
                        if (board.GetCell(new Position(destinationX, destinationY)).GetPlayerGuid() == null)
                        {
                            position.ChangePosition(x);
                            position.ChangePosition(y);
                        }

                    }


                    break;



        }


        }

        private void Discover()
        {

        }

        public void MakeAction()
        {

        }

        public void listen()
        {

        }


        private void TakePiece(bool piece)
        {
            CellState cellState = board.GetCell(position).GetCellState();
            if (cellState == CellState.Piece)
            {
                this.piece = true;
                this.pieceIsSham = false;
            }
            else if (cellState == CellState.Sham)
            {
                this.piece = true;
                this.pieceIsSham = true;
            }
            else
            {
                this.piece = false;
                this.pieceIsSham = false;
            }
        }

        private void TestPiece()
        {

        }

        private void PlacePiece()
        {
            if((team.color==TeamColor.Red && position.y<board.goalAreaHeight)||
                (team.color == TeamColor.Red && position.y >= board.boardHeight-board.goalAreaHeight))
            {
                if(board.GetCell(position).GetCellState()==CellState.Valid&&piece&&!pieceIsSham)
                {
                    Cell cell = board.GetCell(position);
                    cell.SetCellState(CellState.Goal);
                }
            }
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
