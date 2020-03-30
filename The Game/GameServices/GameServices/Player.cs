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

        public void Move(Direction x, Direction y)
        {
            int destinationX = position.x;
            int destinationY = position.y;
            if(x==Direction.Right)
            {
                destinationX++;
            }
            else if(x==Direction.Left)
            {
                destinationX--;
            }
            if (y == Direction.Down)
            {
                destinationY++;
            }
            else if (y == Direction.Up)
            {
                destinationY--;
            }

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


        public void TakePiece()
        {
            Cell cell;
            CellState cellState = board.GetCell(position).GetCellState();
            if (cellState == CellState.Piece)
            {

                cell = board.GetCell(position);
                this.piece = true;
                this.pieceIsSham = false;
                cell.SetCellState(CellState.Empty);
                board.UpdateCell(cell, position);
            }
            else if (cellState == CellState.Sham)
            {
                cell = board.GetCell(position);
                this.piece = true;
                this.pieceIsSham = true;
                cell.SetCellState(CellState.Empty);
                board.UpdateCell(cell, position);
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

        public void PlacePiece()
        {
            if((team.color==TeamColor.Red && position.y<board.goalAreaHeight)||
                (team.color == TeamColor.Blue && position.y >= board.boardHeight-board.goalAreaHeight))
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
