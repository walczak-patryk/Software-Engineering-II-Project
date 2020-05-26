using CommunicationServerLibrary.Interfaces;
using CommunicationServerLibrary.Messages;
using GameMaster.Boards;
using GameMaster.Cells;
using GameMaster.Positions;
using GameMaster.Fields;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Net;
using CommunicationServerLibrary.Adapters;

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
        private int serverPort;
        private IPAddress serverAddress;
        private IConnectionClient connection;
        public PlayerGuid playerGuid;
        private PlayerState state;

        public int turnsSinceDiscover;

        public void Start(string ip, string port)
        {          
            Console.WriteLine("Starting agent launcher");
            ClientLauncher(ip, Int32.Parse(port));

            Message join = new ConnectPlayerMsg("13000", playerGuid);
            Console.WriteLine("I'm sending to GM: " + join.ToString());
            SendMessageToServer(join);
            Console.WriteLine("Join sent");
            Message resp = GetMessageFromServer();
            Console.WriteLine("I received from GM: " + resp.ToString());

            Message ready = new ReadyMsg(playerGuid);
            Console.WriteLine("I'm sending to GM: " + ready.ToString());
            SendMessageToServer(ready);
            Console.WriteLine("Ready sent");
            resp = GetMessageFromServer();
            Console.WriteLine("I received from GM: " + resp.ToString());

            while (true)
            {
                Console.WriteLine("Waiting for start of game...");
                resp = GetMessageFromServer();
                if (resp.GetType() == typeof(GameStartMsg))
                {
                    ProcessMessage(resp);
                    Console.WriteLine("Gamestart Received");
                    break;
                }
            }

            while (true)
            {
                Message action = AIMove();
                Console.WriteLine("I'm sending to GM: " + action.ToString());

                SendMessageToServer(action);
                Console.WriteLine("Player sent");
                Message response = GetMessageFromServer();
                Console.WriteLine("I received from GM: " + response.ToString());
                ProcessMessage(response);

                /*if (response.Split("_").Length < 3)
                    Console.WriteLine("Error while receiveing response from GM");
                else
                {
                    string[] responses = response.Split("_");
                    if (responses[1] == "4")
                    {
                        turnsSinceDiscover = 0;
                        //Discover(ParseDiscover(responses[2]));
                    }
                    else if(responses[1] == "0")
                    {
                        turnsSinceDiscover++;
                        if (responses[3] == "OK")
                        {
                            Move((Direction)int.Parse(action.Split("_")[1]));
                        }
                    }
                    else if(responses[1] == "1")
                    {
                        turnsSinceDiscover++;
                        if (responses[2] == "T")
                            board.cellsGrid[position.x, position.y].SetCellState(CellState.Piece);
                        else
                            board.cellsGrid[position.x, position.y].SetCellState(CellState.Sham);
                        TakePiece();
                    }
                    else if(responses[1] == "2")
                    {
                        turnsSinceDiscover++;
                        TestPiece();
                    }
                    else if(responses[1] == "3")
                    {
                        turnsSinceDiscover++;
                        PlacePiece();
                    }
                }*/
            }
        }

        private void ClientLauncher(string serverAddress, int serverPort)
        {
            this.serverPort = serverPort;
            this.serverAddress = IPAddress.Parse(serverAddress);
            this.connection = new TCPClientAdapter();
            this.connection.Connect(this.serverAddress, serverPort);
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
            this.playerGuid = new PlayerGuid();
            this.turnsSinceDiscover = 100;
            this.isDiscovered = false;
        }

        public void Move(Direction x)
        {
            turnsSinceDiscover++;
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
            if (distancesString.Length == 10)
            {
                foreach (string s in distancesString)
                {
                    if(s != "")
                        distances.Add(int.Parse(s));
                }
            }
            return distances;
        }

        private void Discover(List<Field> fields)
        {
            foreach (Field f in fields)
            {
                if(f.position.x >= 0 && f.position.x < board.boardWidth && f.position.y >= 0 && f.position.y < board.boardHeight)
                {
                    board.GetCell(f.position).SetCellState(f.cell.GetCellState());
                    board.GetCell(f.position).SetDistance(f.cell.GetDistance());
                    board.GetCell(f.position).SetPlayerGuid(f.cell.GetPlayerGuid());
                }
            }
            turnsSinceDiscover = 0;
        }

        public void TakePiece()
        {
            Cell cell;
            CellState cellState = board.GetCell(position).GetCellState();
            if (cellState == CellState.Piece)
            {

                cell = board.GetCell(position);
                this.piece = true;
                //this.pieceIsSham = false;
                cell.SetCellState(CellState.Empty);
                board.UpdateCell(cell, position);
            }
            /*else if (cellState == CellState.Sham)
            {
                cell = board.GetCell(position);
                this.piece = true;
                this.pieceIsSham = true;
                cell.SetCellState(CellState.Empty);
                board.UpdateCell(cell, position);
            }*/
            else
            {
                this.piece = false;
                //this.pieceIsSham = false;
            }
            this.isDiscovered = false;

        }

        private void TestPiece(bool? status)
        {
            if (this.piece)
            {
                if (status == true)
                {
                    this.pieceIsSham = false;
                    this.isDiscovered = true;
                    return;
                }
                else if (status == false)
                {
                    this.piece = false;
                    this.pieceIsSham = true;
                    this.isDiscovered = false;
                    turnsSinceDiscover = 1;
                    return;
                }
            }
        }

        public void PlacePiece(PlacementResult res)
        {
            if((team.color==TeamColor.Red && position.y<board.goalAreaHeight)||
                (team.color == TeamColor.Blue && position.y >= board.boardHeight-board.goalAreaHeight))
            {
                Cell cell = board.GetCell(position);
                if (res == PlacementResult.Correct)
                    cell.SetCellState(CellState.Goal);
                else
                    cell.SetCellState(CellState.NoGoal);
                piece = false;
                pieceIsSham = false;
            }
        }


        Message AIMove() //simple AI for player
        {
            Random rand = new Random();
            if (piece == false)
            {
                if (turnsSinceDiscover > 0)
                {
                    return new DiscoverMsg(playerGuid, position);
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
                                    return new MoveMsg(playerGuid, Direction.Left);
                                }

                                else
                                {
                                    return new MoveMsg(playerGuid, Direction.Up);
                                }
                            }
                        case 1:
                            {
                                return new MoveMsg(playerGuid, Direction.Up);
                            }
                        case 2:
                            {
                                if (rand.Next() % 2 == 0)
                                {
                                    return new MoveMsg(playerGuid, Direction.Right);
                                }
                                else
                                {
                                    return new MoveMsg(playerGuid, Direction.Up);
                                }
                            }
                        case 3:
                            {
                                return new MoveMsg(playerGuid, Direction.Left);
                            }
                        case 4:
                            {
                                return new PickUpMsg(playerGuid);
                            }
                        case 5:
                            {
                                return new MoveMsg(playerGuid, Direction.Right);
                            }
                        case 6:
                            {
                                if (rand.Next() % 2 == 0)
                                {
                                    return new MoveMsg(playerGuid, Direction.Left);
                                }
                                else
                                {
                                    return new MoveMsg(playerGuid, Direction.Down);
                                }
                            }
                        case 7:
                            {
                                return new MoveMsg(playerGuid, Direction.Down);
                            }
                        case 8:
                            {
                                if (rand.Next() % 2 == 0)
                                {
                                    return new MoveMsg(playerGuid, Direction.Right);
                                }
                                else
                                {
                                    return new MoveMsg(playerGuid, Direction.Down);
                                }
                            }
                        default: return new Message("Unknown");
                    }
                }
            }
            else
            {
                if (!isDiscovered)
                {
                    return new TestMsg(playerGuid);
                }
                else
                {
                    if (team.getColor() == TeamColor.Blue)
                    {
                        if (position.y < board.taskAreaHeight + board.goalAreaHeight)
                        {
                            return new MoveMsg(playerGuid, Direction.Down);
                        }
                        else
                        {
                            if (board.GetCell(position).GetCellState() == CellState.Goal)
                            {
                                int dir = rand.Next() % 3;
                                return new MoveMsg(playerGuid, Direction.Up + dir);
                            }
                            else
                            {
                                return new PlaceMsg(playerGuid);
                            }
                        }
                    }
                    else
                    {
                        if (position.y > board.goalAreaHeight)
                        {
                            return new MoveMsg(playerGuid, Direction.Up);
                        }
                        else
                        {
                            if (board.GetCell(position).GetCellState() == CellState.Goal)
                            {
                                int dir = rand.Next() % 3;
                                return new MoveMsg(playerGuid, Direction.Up + dir);
                            }
                            else
                            {
                                return new PlaceMsg(playerGuid);
                            }
                        }
                    }
                }
            }
        }
        private Message GetMessageFromServer()
        {
            Message msg = connection.GetMessage();
            return msg;
        }

        private void SendMessageToServer(Message m)
        {
            if (!connection.SendMessage(m))
            {
                throw new Exception("Connection lost, can't send message");
            }
        }
        private void ProcessMessage(Message m)
        {
            switch(m)
            {
                case MoveResMsg _:
                    MoveResMsg resMove = (MoveResMsg)m;
                    if (resMove.status == "OK")
                        Move(resMove.direction);
                    break;
                case PickUpResMsg _:
                    //DO POPRAWY LOGIKA
                    PickUpResMsg resPick = (PickUpResMsg)m;
                    if (resPick.status == "OK")
                        TakePiece();
                    break;
                case TestResMsg _:
                    //DO POPRAWY LOGIKA
                    TestResMsg resTest = (TestResMsg)m;
                    if (resTest.status == "OK")
                        TestPiece(resTest.test);
                    break;
                case PlaceResMsg _:
                    //DO POPRAWY LOGIKA
                    PlaceResMsg resPlace = (PlaceResMsg)m;
                    if (resPlace.status == "OK")
                    {
                        if(resPlace.placementResult == "Correct")
                            PlacePiece(PlacementResult.Correct);
                        else
                            PlacePiece(PlacementResult.Pointless);
                    }
                    break;
                case DiscoverResMsg _:
                    DiscoverResMsg resDiscover = (DiscoverResMsg)m;
                    if (resDiscover.status == "OK")
                        Discover(resDiscover.fields);
                    break;
                case ConnectPlayerResMsg _:
                    break;
                case ReadyResMsg _:
                    break;
                case SetupResMsg _:
                    break;
                case GameStartMsg _:
                    GameStartMsg gameStart = (GameStartMsg)m;
                    position = gameStart.position;
                    board = gameStart.board;
                    team.color = gameStart.team;
                    if (gameStart.teamRole == TeamRole.Leader)
                        isLeader = true;
                    else
                        isLeader = false;
                    break;
                default:
                    break;
            }
        }
    }

    public enum PlayerState
    {
        Initializing,
        Active,
        Completed
    }   
}
