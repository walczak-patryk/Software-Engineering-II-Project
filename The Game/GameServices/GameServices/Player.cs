using GameMaster.Boards;
using GameMaster.Cells;
using GameMaster.Positions;
using System;
using System.Collections.Generic;
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
        public bool isDiscovered;
        public Board board;
        public ActionType lastAction;
        public string guid;
        private PlayerState state;

        public int turnsSinceDiscover;

        public static void Main()
        {
            Console.WriteLine("Player");
            var p = new Player(1, new Team(), false);
            p.SendToGM("I work");
            while (true)
            {
                string message = p.id.ToString() +  "_";
                string action = p.AIMove();
                message += action;
                p.SendToGM(message);
                Console.WriteLine("I'm sending to GM: " + message);
                string response = p.ReceiveFromGM();
                Console.WriteLine("I received from GM: " + response);
                if (response.Split("_").Length < 3)
                    Console.WriteLine("Error while receiveing response from GM");
                else
                {
                    string[] responses = response.Split("_");
                    if (responses[1] == "4")
                    {
                        p.Discover(p.ParseDiscover(responses[2]));
                        p.turnsSinceDiscover = 0;
                    }
                    else if(responses[1] == "0")
                    {
                        if (responses[2] == "OK")
                        {
                            p.Move((Direction)int.Parse(action.Split("_")[1]));
                            p.turnsSinceDiscover++;
                        }
                    }
                    else if(responses[1] == "1")
                    {
                        if (responses[2] == "T")
                            p.board.cellsGrid[p.position.x, p.position.y].SetCellState(CellState.Piece);
                        else
                            p.board.cellsGrid[p.position.x, p.position.y].SetCellState(CellState.Sham);
                        p.TakePiece();
                        p.turnsSinceDiscover++;
                    }
                    else if(responses[1] == "2")
                    {
                        p.TestPiece();
                        p.turnsSinceDiscover++;
                    }
                    else if(responses[1] == "3")
                    {
                        p.PlacePiece();
                        p.turnsSinceDiscover++;
                    }
                }
            }
            //Console.ReadLine();
            //TODO player
        }

        public Player(int Id, Team Team, bool IsLeader)
        {
            this.id = Id;
            this.team = Team;
            this.isLeader = IsLeader;
            this.position = new Position(0,0);
            this.piece = false;
            this.pieceIsSham = false;
            this.board = new Board(0,0,0);
            this.guid = Team.getColor().ToString()[0] + Id.ToString();
            this.turnsSinceDiscover = 100;
            this.isDiscovered = false;
        }


        #region Communication wih GM

        public string ReturnPath()
        {
            return Environment.CurrentDirectory;
        }
        private string ReceiveFromGM()
        {

                using (NamedPipeServerStream pipeServer = new NamedPipeServerStream("Player_Pipe_Server" + guid, PipeDirection.In))
                {
                    pipeServer.WaitForConnection();

                    using (StreamReader sr = new StreamReader(pipeServer))
                    {
                        return sr.ReadLine();
                        //string temp;
                        //while ((temp = sr.ReadLine()) != null)
                        //{
                        //    Console.WriteLine("Received from server: {0}", temp);
                        //}
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

        public List<int> ParseDiscover(string response)
        {
            List<int> distances = new List<int>();
            string[] distancesString = response.Split(",");
            foreach (string s in distancesString)
            {
                distances.Add(int.Parse(s));
            }
            return distances;
        }

        private void Discover(List<int> distances)
        {
            for (int index = 0, j = -1; j <= 1; j++, index++)
            {
                for (int i = -1; i <= 1; i++, index++)
                {
                    if (!(position.x + i < 0 || position.y + j < 0 || position.x + i > board.boardWidth || position.y + j > board.boardHeight))
                        board.cellsGrid[position.x + i, position.y + j].SetDistance(distances[index]);
                }
            }
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
            this.isDiscovered = false;

        }

        private void TestPiece()
        {
            if(this.piece==true)
            {
                this.isDiscovered = true;
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


        string AIMove() //simple AI for player
        {
            Random rand = new Random();
            if (piece == false)
            {
                if (turnsSinceDiscover > 0)
                {
                    return "4";
                }
                else
                {
                    List<int> distances = new List<int>();
                    for (int j = -1; j <= 1; j++)
                    {
                        for (int i = -1; i <= 1; i++)
                        {
                            if (position.x + i < 0 || position.y + j < 0 || position.x + i > board.boardWidth - 1 || position.y + j > board.boardHeight - 1)
                                distances.Add(Math.Max(board.boardWidth, board.boardHeight));
                            else
                                distances.Add(board.cellsGrid[position.x + i, position.y + j].GetDistance());
                        }
                    }
                    int min = distances[0];
                    int dir = 0;
                    for (int i = 1; i < distances.Count; i++)
                    {
                        if (distances[i] < min)
                        {
                            min = distances[i];
                            dir = i;
                        }
                    }
                    switch (dir)
                    {
                        case 0:
                            {
                                if (rand.Next() % 2 == 0)
                                {
                                    return "0_2";
                                }

                                else
                                {
                                    return "0_0";
                                }
                            }
                        case 1:
                            {
                                return "0_0";
                            }
                        case 2:
                            {
                                if (rand.Next() % 2 == 0)
                                {
                                    return "0_3";
                                }
                                else
                                {
                                    return "0_0";
                                }
                            }
                        case 3:
                            {
                                return "0_2";
                            }
                        case 4:
                            {
                                return "1";
                            }
                        case 5:
                            {
                                return "0_3";
                            }
                        case 6:
                            {
                                if (rand.Next() % 2 == 0)
                                {
                                    return "0_2";
                                }
                                else
                                {
                                    return "0_1";
                                }
                            }
                        case 7:
                            {
                                return "0_1";
                            }
                        case 8:
                            {
                                if (rand.Next() % 2 == 0)
                                {
                                    return "0_3";
                                }
                                else
                                {
                                    return "0_1";
                                }
                            }
                        default: return "";
                    }
                }
            }
            else
            {
                if (!isDiscovered)
                {
                    return "2";
                }
                else
                {
                    if (team.getColor() == TeamColor.Blue)
                    {
                        if (position.y < board.taskAreaHeight + board.goalAreaHeight)
                        {
                            return "0_0";
                        }
                        else
                        {
                            if (board.GetCell(position).GetCellState() == CellState.Goal)
                            {
                                int dir = rand.Next() % 3;
                                return "0_" + dir.ToString();
                            }
                            else
                            {
                                return "3";
                            }
                        }
                    }
                    else
                    {
                        if (position.y > board.goalAreaHeight)
                        {
                            return "0_1";
                        }
                        else
                        {
                            if (board.GetCell(position).GetCellState() == CellState.Goal)
                            {
                                int dir = rand.Next() % 3;
                                return "0_" + dir.ToString();
                            }
                            else
                            {
                                return "3";
                            }
                        }
                    }
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
        Discover,
        Destroy,
        Send
    }
}
