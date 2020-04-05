using GameMaster.Boards;
using GameMaster.Cells;
using GameMaster.Positions;
using System;
using System.IO;
using System.IO.Pipes;

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
        public string guid;
        private PlayerState state;
        private string pipe;

        public static void Main()
        {
            Console.WriteLine("Player");
            var p = new Player(1, "1", new Team(), false);
            p.SendToGM("I work");
            Console.ReadLine();
            //TODO player
        }

        public Player(int Id, string name, Team Team, bool IsLeader)
        {
            this.id = Id;
            this.team = Team;
            this.isLeader = IsLeader;
            this.position = new Position(0,0);
            this.piece = false;
            this.pieceIsSham = false;
            this.board = new Board(0,0,0);
            this.guid = Team.getColor().ToString()[0] + Id.ToString(); 
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

        #region Communication wih GM

        public string ReturnPath()
        {
            return Environment.CurrentDirectory;
        }
        private void ReceiveFromGM()
        {
            if (this.pipe != "" && this.pipe != null)
            {
                using (NamedPipeServerStream pipeServer =
           new NamedPipeServerStream("Player_Pipe_Server" + this.pipe, PipeDirection.In))
                {
                    pipeServer.WaitForConnection();

                    using (StreamReader sr = new StreamReader(pipeServer))
                    {
                        string temp;
                        while ((temp = sr.ReadLine()) != null)
                        {
                            Console.WriteLine("Received from server: {0}", temp);
                        }
                    }
                }
            }
        }

        public void SendToGM(string message)
        {
            using (NamedPipeClientStream pipeClient =
            new NamedPipeClientStream(".", "GM_Pipe_Server", PipeDirection.Out))
            {
                pipeClient.Connect();
                try
                {
                    using (StreamWriter sw = new StreamWriter(pipeClient))
                    {
                        sw.AutoFlush = true;
                        sw.WriteLine(message);
                    }
                }
                catch (IOException e)
                {
                    Console.WriteLine("ERROR: {0}", e.Message);
                }
            }
        }
        #endregion

        public void Move(Direction x)
        {
            int destinationX = position.x;
            int destinationY = position.y;
            if (x == Direction.Right)
            {
                destinationX++;
            }
            else if (x == Direction.Left)
            {
                destinationX--;
            }
            else if (x == Direction.Down)
            {
                destinationY++;
            }
            else if (x == Direction.Up)
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
                            board.GetCell(position).SetPlayerGuid(null);
                            position.ChangePosition(x);
                            board.GetCell(position).SetPlayerGuid(guid);
                        }

                    }
                    break;
                case TeamColor.Blue:
                    if (0 <= destinationX && destinationX < board.boardWidth && board.goalAreaHeight <= destinationY && destinationY < board.boardHeight)
                    {
                        if (board.GetCell(new Position(destinationX, destinationY)).GetPlayerGuid() == null)
                        {
                            board.GetCell(position).SetPlayerGuid(null);
                            position.ChangePosition(x);
                            board.GetCell(position).SetPlayerGuid(guid);
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
            if(this.piece==true)
            {
                return;
            }
            if(this.pieceIsSham==false)
            {
                this.piece = false;
            }
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
