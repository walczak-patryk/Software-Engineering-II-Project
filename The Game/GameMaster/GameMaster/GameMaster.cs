using CommunicationServerLibrary.Adapters;
using CommunicationServerLibrary.Interfaces;
using CommunicationServerLibrary.Messages;
using GameMaster.Boards;
using GameMaster.Cells;
using GameMaster.Fields;
using GameMaster.Positions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace GameMaster
{
    public class GameMaster
    {
        private int portNumber;
        private IPAddress IPAddress;
        public GameMasterBoard board;
        private readonly GameMasterStatus status;
        private readonly GameMasterConfiguration configuration;
        public List<PlayerGuid> teamRedGuids;
        public List<PlayerGuid> teamBlueGuids;
        public IConnectionClient connectionClient;
        private string winTeam;

        private Process GuiWindow;
        private bool isGuiWorking;

        private bool debug;

        public GameMaster()
        {
            this.configuration = new GameMasterConfiguration();
            this.connectionClient = new TCPClientAdapter();
            this.status = GameMasterStatus.Active;
            teamBlueGuids = new List<PlayerGuid>();
            teamRedGuids = new List<PlayerGuid>();
            isGuiWorking = false;
            debug = true;
        }
        public void Run()
        {
            //StartGUIAsync();
            Task g = Task.Run(() =>
            {
                this.ReceiveFromGUI();
                if (isGuiWorking)
                {
                    TestMessageToGui();
                }
            });

            try
            {
                if (!ConnectToCommunicationServer())
                    return;
                WaitForPlayersToConnect();
                SendGameStartMsg();
                HandleActions();
                SendGameOverMsg();
                DisconnectCommunicationServer();
            }
            catch (Exception e)
            {
                System.Console.Out.WriteLine($"{DateTime.Now}:{e.GetType().ToString()} was thrown.\nValue: {e.HResult}\nMessage: {e.Message}\nSource: {e.StackTrace}\n\nGM stopped working");
            }
        }
        public void EndGame()
        {
            if (this.GuiWindow != null)
                this.GuiWindow.Kill();
        }

        #region GUI Managment
        private void StartGUIAsync()
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "GameGraphicalInterface.exe",
                //psi.Arguments = new MainWindow(board, configuration.shamProbability, configuration.maxPieces, configuration.initialPieces, configuration.predefinedGoalPositions).ReturnPath();
                UseShellExecute = true
            };
            this.GuiWindow = Process.Start(psi);
        }

        private void ReceiveFromGUI()   
        {
            using NamedPipeServerStream pipeServer =
            new NamedPipeServerStream("GM_Pipe_Server", PipeDirection.In);
            pipeServer.WaitForConnection();

            using StreamReader sr = new StreamReader(pipeServer);
            string temp;
            while ((temp = sr.ReadLine()) != null)
            {
                Console.WriteLine("Received from server: {0}", temp);
                if ("1_1" == temp)
                {
                    isGuiWorking = true;
                    this.SendToGUI(this.MessageOptionsForGUI());
                }
                if ("1_0" == temp)
                    isGuiWorking = false;
            }
        }

        public void SendToGUI(string message)
        {
            using NamedPipeClientStream pipeClient =
            new NamedPipeClientStream(".", "GUI_Pipe_Server", PipeDirection.Out);
            pipeClient.Connect();
            try
            {
                using StreamWriter sw = new StreamWriter(pipeClient)
                {
                    AutoFlush = true
                };
                sw.WriteLine(message);
            }
            catch (IOException e)
            {
                Console.WriteLine("ERROR: {0}", e.Message);
            }
        }
        #endregion
        #region Players Managment

        Message ParsePlayerAction(Message m)
        {
            return m switch
            {
                MoveMsg _ => DecideMove((MoveMsg)m),
                PickUpMsg _ => DecideTake((PickUpMsg)m),
                TestMsg _ => DecideTest((TestMsg)m),
                PlaceMsg _ => DecidePlace((PlaceMsg)m),
                DiscoverMsg _ => DecideDiscover((DiscoverMsg)m),
                _ => new Message("Unknown"),
            };
        }

        Message DecideMove(MoveMsg m)
        {
            if(debug)
                Console.WriteLine("DEBUG: player {0} position - {1} , {2} - move {3}", m.playerGuid.g.ToString(), FindPlayer(m.playerGuid.g.ToString()).x, FindPlayer(m.playerGuid.g.ToString()).y, m.direction.ToString());
            
            if (Move(m.playerGuid, m.direction))
                return new MoveResMsg(m.playerGuid, m.direction, "OK", FindPlayer(m.playerGuid.g.ToString()));
            else
                return new MoveResMsg(m.playerGuid, m.direction, "DENIED", FindPlayer(m.playerGuid.g.ToString()));
        }

        Message DecideTake(PickUpMsg m)
        {
            if (debug)
                Console.WriteLine("DEBUG: player {0} position - {1} , {2} - take", m.playerGuid.g.ToString(), FindPlayer(m.playerGuid.g.ToString()).x, FindPlayer(m.playerGuid.g.ToString()).y);

            if (TakePiece(m.playerGuid.g.ToString()))
                return new PickUpResMsg(m.playerGuid, "OK");
            else
                return new PickUpResMsg(m.playerGuid, "DENIED");
        }

        Message DecideTest(TestMsg m)
        {
            if (debug)
                Console.WriteLine("DEBUG: player {0} position - {1} , {2} - test", m.playerGuid.g.ToString(), FindPlayer(m.playerGuid.g.ToString()).x, FindPlayer(m.playerGuid.g.ToString()).y);

            if (TestPiece())
                return new TestResMsg(m.playerGuid, true, "OK");
            else
                return new TestResMsg(m.playerGuid, false, "OK");
        }

        Message DecideDiscover(DiscoverMsg m)
        {
            if (debug)
                Console.WriteLine("DEBUG: player {0} position - {1} , {2} - discover", m.playerGuid.g.ToString(), FindPlayer(m.playerGuid.g.ToString()).x, FindPlayer(m.playerGuid.g.ToString()).y);

            System.Threading.Thread.Sleep(configuration.delayDiscover);
            Position playerPosition = FindPlayer(m.playerGuid.g.ToString());
            if (playerPosition == null)
            {
                Console.WriteLine("Player with guid {0} not found", m.playerGuid.g.ToString());
                return new DiscoverResMsg(m.playerGuid, FindPlayer(m.playerGuid.g.ToString()), new List<Fields.Field>(), "DENIED");
            }
            else
            {
                List<Field> list = board.Discover(playerPosition);
                return new DiscoverResMsg(m.playerGuid, FindPlayer(m.playerGuid.g.ToString()), list, "OK");
            }
        }

        Message DecidePlace(PlaceMsg m)
        {
            if (debug)
                Console.WriteLine("DEBUG: player {0} position - {1} , {2} - place", m.playerGuid.g.ToString(), FindPlayer(m.playerGuid.g.ToString()).x, FindPlayer(m.playerGuid.g.ToString()).y);

            if (PlacePiece(m.playerGuid))
                return new PlaceResMsg(m.playerGuid, "Correct", "OK");
            else
                return new PlaceResMsg(m.playerGuid, "Pointless", "OK");
        }
        #endregion
        #region Server Communication
        private bool SendMessage(Message msg)
        {
            bool isStatusOk = connectionClient.SendMessage(msg);
            if (!isStatusOk)
                Logger.Error($"Problem occurred when sending message {msg.GetType().Name} to player");
            return isStatusOk;
        }
        private Message GetMessage()
        {
            try
            {
                Message msg = connectionClient.GetMessage();
                if (msg is null)
                {
                    Logger.Warning("No mesagge");
                    throw new Exception("Message is null");
                }
                return msg;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        private bool ConnectToCommunicationServer()
        {
            IPAddress = IPAddress.Parse("127.0.0.1");
            portNumber = 13000;
            Logger.Log($"Connecting to communication server at {IPAddress}:{portNumber}");
            if (!connectionClient.Connect(IPAddress, portNumber))
            {
                Logger.Error("TCP error");
                return false;
            }
            Logger.Log("Connection established");

            SendMessage(new ConnectGMMsg(portNumber.ToString()));
            Message msg = GetMessage();
            if (!(msg is ConnectGMResMsg))
            {
                Logger.Error($"Unexpected message received from CS: '{msg}'");
                return false;
            }
            return true;
        }
        public bool IsTeamsReady()
        {
            if (teamBlueGuids.Count() == configuration.maxTeamSize && teamRedGuids.Count() == configuration.maxTeamSize)
                return true;
            return false;
        }

        private void AddPlayer(PlayerGuid playerGuid)
        {
            Random random = new Random();
            var n = random.Next(0, 2);
            if (n == 0)
            {
                if (teamBlueGuids.Count() < configuration.maxTeamSize)
                {
                    teamBlueGuids.Add(playerGuid);
                    Cell cell = new Cell(1);
                    cell.SetPlayerGuid(playerGuid.g.ToString());
                    board.UpdateCell(cell, new Position(1, board.taskAreaHeight + board.goalAreaHeight + 1));
                }
                else
                {
                    teamRedGuids.Add(playerGuid);
                    Cell cell = new Cell(1);
                    cell.SetPlayerGuid(playerGuid.g.ToString());
                    board.UpdateCell(cell, new Position(1, 1));
                }
            }
            else
            {
                if (teamRedGuids.Count() < configuration.maxTeamSize)
                {
                    teamRedGuids.Add(playerGuid);
                    Cell cell = new Cell(1);
                    cell.SetPlayerGuid(playerGuid.g.ToString());
                    board.UpdateCell(cell, new Position(1, 1));
                }
                else
                {
                    teamBlueGuids.Add(playerGuid);
                    Cell cell = new Cell(1);
                    cell.SetPlayerGuid(playerGuid.g.ToString());
                    board.UpdateCell(cell, new Position(1, board.taskAreaHeight + board.goalAreaHeight + 1));
                }
            }
        }

        private void WaitForPlayersToConnect()
        {
            Logger.Log("Waiting for players");
            Message msg;
            this.board = new GameMasterBoard(this.configuration.boardGoalHeight, this.configuration.boardGoalHeight, this.configuration.boardTaskHeight, this.configuration.predefinedGoalPositions);

            //Message response;

            while (!IsTeamsReady())
            {
                msg = GetMessage();
                if (!(msg is ConnectPlayerMsg connectPlayerMsg))
                {
                    Logger.Error($"Unexpected message received: '{msg}'");
                    continue;
                }
                AddPlayer(connectPlayerMsg.playerGuid);
                Logger.Log($"Connect request: {connectPlayerMsg.playerGuid.g}\tStatus -> OK");
                SendMessage(new ConnectPlayerResMsg(connectPlayerMsg.portNumber, connectPlayerMsg.playerGuid, "OK"));
            }

            Thread.Sleep(2000);
            Logger.Log("All players connected");
            Logger.Log("Game prepared");
        }

        private void SendGameStartMsg()
        {
            var blueGuids = teamBlueGuids.Select(item => item.g.ToString()).ToArray();
            var redGuids = teamRedGuids.Select(item => item.g.ToString()).ToArray();
            foreach(var player in teamBlueGuids)
            {
                var msgBlue = new GameStartMsg(player, TeamColor.Blue, TeamRole.Member, configuration.maxTeamSize, blueGuids, FindPlayer(player.g.ToString()), board);
                SendMessage(msgBlue);
                Logger.Log($"GameStart message sent to player {player.g.ToString()}");
            }            
            foreach(var player in teamRedGuids)
            {
                var msgRed = new GameStartMsg(player, TeamColor.Red, TeamRole.Member, configuration.maxTeamSize, redGuids, FindPlayer(player.g.ToString()), board);
                SendMessage(msgRed);
                Logger.Log($"GameStart message sent to player {player.g.ToString()}");
            }
            SendMessage(new SetupMsg());
        }

        private bool IsEnd()
        {
            if (board.CheckWinCondition(TeamColor.Red))
            {
                Logger.Log("Red team wins.");
                winTeam = "red";
                return true;
            }
            else if (board.CheckWinCondition(TeamColor.Blue))
            {
                Logger.Log("Blue team wins.");
                winTeam = "blue";
                return true;
            }
            return false;     
        }
        private void HandleActions()
        {
            Logger.Log("Handling actions (entering loop).");
            while (!IsEnd())
            {
                Logger.Log("Handling actions. " + DateTime.Now);
                Message msg = GetMessage();
                if (msg != null)
                {
                    var response = ParsePlayerAction(msg);
                    SendMessage(response);
                }
                else
                    Thread.Sleep(300);
            }
        }
        private void SendGameOverMsg()
        {
            var msg = new GameEndMsg(TeamColor.Blue);
            foreach (var player in teamBlueGuids)
            {
                SendMessage(msg);
                Logger.Log($"GameEnd message sent to player {player.g.ToString()}");
            }
            foreach (var player in teamRedGuids)
            {
                SendMessage(msg);
                Logger.Log($"GameEnd message sent to player {player.g.ToString()}");
            }
        }
        private void DisconnectCommunicationServer()
        {
            if (connectionClient.IsConnected())
                connectionClient.SafeDisconnect();
        }
        #endregion
        public GameMasterConfiguration LoadConfigurationFromJSON(string path)
        {
            return null;
        }

        public bool TakePiece(string playerGUID)
        {
            System.Threading.Thread.Sleep(configuration.delayPick);
            foreach (var elem in board.cellsGrid)
            {
                if(elem.GetPlayerGuid()==playerGUID)
                {
                    if(elem.GetCellState() == CellState.Piece)
                    {
                        elem.SetCellState(CellState.Empty);
                        return true;
                    }
                }
            }
            return false;
        }

        public bool TestPiece()
        {
            System.Threading.Thread.Sleep(configuration.delayTest);
            Random rand = new Random();
            if (rand.NextDouble() < configuration.shamProbability)
                return false;
            return true;
        }

        private Position FindPlayer(string playerGUID)
        {
            for (int x = 0; x < board.boardWidth; x++)
            {
                for (int y = 0; y < board.boardHeight; y++)
                {
                    if (board.cellsGrid[x, y].GetPlayerGuid() == playerGUID)
                    {
                        return new Position(x, y);
                    }
                }
            }
            return null;
        }
        public bool Move(PlayerGuid playerGUID, Direction direction)
        {
            System.Threading.Thread.Sleep(configuration.delayMove);
            Position playerPosition = FindPlayer(playerGUID.g.ToString());
            int destinationX = playerPosition.x;
            int destinationY = playerPosition.y;
            if (direction == Direction.Right)
            {
                destinationX++;
            }
            else if (direction == Direction.Left)
            {
                destinationX--;
            }
            else if (direction == Direction.Down)
            {
                destinationY++;
            }
            else if (direction == Direction.Up)
            {
                destinationY--;
            }
            TeamColor teamColor;

            if(teamRedGuids.Contains(playerGUID))
            {
                teamColor = TeamColor.Red;
            }
            else
            {
                teamColor = TeamColor.Blue;
            }

            Position destinationPosition = new Position(destinationX, destinationY);
            switch (teamColor)
            {
                case TeamColor.Red:
                    if (0 <= destinationX && destinationX < board.boardWidth
                        && 0 <= destinationY && destinationY < board.boardHeight - board.goalAreaHeight)
                    {
                        if (board.GetCell(destinationPosition).GetPlayerGuid() == null)
                        {
                            board.GetCell(playerPosition).SetPlayerGuid(null);
                            board.GetCell(destinationPosition).SetPlayerGuid(playerGUID.g.ToString());
                            return true;
                        }

                    }
                    break;
                case TeamColor.Blue:
                    if (0 <= destinationX && destinationX < board.boardWidth && board.goalAreaHeight <= destinationY && destinationY < board.boardHeight)
                    {
                        if (board.GetCell(destinationPosition).GetPlayerGuid() == null)
                        {
                            board.GetCell(playerPosition).SetPlayerGuid(null);
                            board.GetCell(destinationPosition).SetPlayerGuid(playerGUID.g.ToString());
                            return true;
                        }

                    }
                    break;
            }
            return false;
        }

        public bool PlacePiece(PlayerGuid playerGUID)
        {
            System.Threading.Thread.Sleep(configuration.delayNextPiecePlace);
            Position playerPosition = FindPlayer(playerGUID.g.ToString());

            TeamColor teamColor;
            if (teamRedGuids.Contains(playerGUID))
            {
                teamColor = TeamColor.Red;
            }
            else
            {
                teamColor = TeamColor.Blue;
            }
            Position position = new Position(playerPosition.x, playerPosition.y);
            if ((teamColor == TeamColor.Red && playerPosition.x < board.goalAreaHeight) ||
                (teamColor == TeamColor.Blue && playerPosition.y >= board.boardHeight - board.goalAreaHeight))
            {
                if (board.GetCell(position).GetCellState() == CellState.Valid)
                {
                    Cell cell = board.GetCell(position);
                    cell.SetCellState(CellState.Goal);
                    return true;
                }
            }

            return false;
        }

        public string MessageOptionsForGUI()
        {
            string message = "o;";

            message += "w," + board.boardWidth.ToString() + ";";
            message += "g," + board.goalAreaHeight.ToString() + ";";
            message += "t," + board.taskAreaHeight.ToString() + ";";

            return message;
        }

        public string MessageStateForGUI()
        {
            string message = "s;";

            for(int j = 0; j < board.boardHeight; j++) {
                for(int i = 0; i < board.boardWidth; i++)
                {
                    bool flag = false;
                    for (int k = 0; teamRedGuids != null && k < teamRedGuids.Count && !flag; k++)
                    {
                        if (board.cellsGrid[i, j].GetPlayerGuid() == teamRedGuids[k].ToString())
                        {
                            message += "7,r," + board.cellsGrid[i, j].GetPlayerGuid() + ",";
                            flag = true;
                        }
                    }
                    for (int k = 0; teamBlueGuids != null && k < teamBlueGuids.Count && !flag; k++)
                    {
                        if (board.cellsGrid[i, j].GetPlayerGuid() == teamBlueGuids[k].ToString())
                        {
                            message += "7,b," + board.cellsGrid[i, j].GetPlayerGuid() + ",";
                            flag = true;
                        }
                    }
                    if(!flag)
                        message += ((int)(board.cellsGrid[i, j].GetCellState())).ToString() + ",";
                }
            }

            message = message[0..^1];
            message += ";";

            return message;
        }

        public string MessageEndForGUI() 
        {
            string message = "e;";

            message += "End;";

            return message;
        }

        // for testing
        private void TestMessageToGui()
        {
            Thread.Sleep(5000);
            teamRedGuids.Add(new PlayerGuid());
            this.board.cellsGrid[1, 1].SetPlayerGuid("1");
            teamBlueGuids.Add(new PlayerGuid());
            this.board.cellsGrid[0, 1].SetPlayerGuid("2");
            this.board.cellsGrid[0, 0].SetCellState(CellState.Valid);
            this.board.cellsGrid[0, 4].SetCellState(CellState.Piece);
            this.SendToGUI(MessageStateForGUI());

            Thread.Sleep(5000);
            this.board.cellsGrid[1, 1].SetPlayerGuid("");
            this.board.cellsGrid[0, 1].SetPlayerGuid("");
            this.board.cellsGrid[2, 1].SetPlayerGuid("1");
            this.board.cellsGrid[1, 2].SetPlayerGuid("2");
            this.SendToGUI(MessageStateForGUI());

            Thread.Sleep(5000);
            this.SendToGUI(MessageEndForGUI());

            Thread.Sleep(5000);
            this.board.cellsGrid[1, 1].SetPlayerGuid("1");
            this.board.cellsGrid[0, 1].SetPlayerGuid("2");
            this.board.cellsGrid[2, 1].SetPlayerGuid("");
            this.board.cellsGrid[1, 2].SetPlayerGuid("");
            this.board.cellsGrid[0, 0].SetCellState(CellState.Valid);
            this.board.cellsGrid[0, 4].SetCellState(CellState.Piece);
            this.SendToGUI(MessageStateForGUI());
        }

    }
}
