﻿using GameGraphicalInterface;
using GameMaster.Boards;
using GameMaster.Cells;
using GameMaster.Positions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace GameMaster
{
    public class GameMaster
    {
        private int portNumber;
        private IPAddress IPAddress;
        public GameMasterBoard board;
        private GameMasterStatus status;
        private GameMasterConfiguration configuration;
        public List<string> teamRedGuids;
        public List<string> teamBlueGuids;

        private Process GuiWindow;


        public void StartGame()
        {
            this.configuration = new GameMasterConfiguration();
            this.board = new GameMasterBoard(this.configuration.boardGoalHeight, this.configuration.boardGoalHeight, this.configuration.boardTaskHeight);
            this.status = GameMasterStatus.Active;
            teamBlueGuids = new List<string>();
            teamRedGuids = new List<string>();
            Task.Run(() =>
            {
                while (true)
                {
                    this.ReceiveFromGUI();
                    string message=ReceiveFromPlayer();
                    if(message==null)
                    {
                        continue;
                    }
                    string[] messageParts = message.Split();
                    if(messageParts.Length>0)
                    {
                        string answer=ParsePlayerAction(message);
                        SendToPlayer(answer, messageParts[0]);
                        SendToGUI("0_1");
                    }

                }
            });
            StartGUIAsync();
            StartPlayer();
        }

        public void EndGame()
        {
            if (this.GuiWindow != null)
                this.GuiWindow.Kill();
        }

        #region GUI Managment
        private void StartGUIAsync()
        {
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = "GameGraphicalInterface.exe";
            //psi.Arguments = new MainWindow(board, configuration.shamProbability, configuration.maxPieces, configuration.initialPieces, configuration.predefinedGoalPositions).ReturnPath();
            psi.UseShellExecute = true;
            this.GuiWindow = Process.Start(psi);
        }

        private void ReceiveFromGUI()   
        {
            using (NamedPipeServerStream pipeServer =
            new NamedPipeServerStream("GM_Pipe_Server", PipeDirection.In))
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

        public void SendToGUI(string message)
        {
            using (NamedPipeClientStream pipeClient =
            new NamedPipeClientStream(".", "GUI_Pipe_Server", PipeDirection.Out))
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
        #region Players Managment

        string ParsePlayerAction(string message)
        {
            string[] messages = message.Split("_");
            if (messages.Length < 2)
                Console.WriteLine("Error");
            if (messages[1] == "0")
            {
                if (messages.Length < 3)
                {
                    Console.WriteLine("Error Move");
                    return "NO";
                }
                return DecideMove(messages[0], messages[2]);
            }
            else if (messages[1] == "1")
            {
                return DecideTake(messages[0]);
            }
            else if (messages[1] == "2")
            {
                return "OK";
            }
            else if (messages[1] == "3")
            {
                return DecidePlace(messages[0]);
            }
            else if (messages[1] == "4")
            {
                return DecideDiscover(messages[0]);
            }
            else
                return "NO";
        }

        string DecideMove(string guid, string dir)
        {
            if (dir != "0" && dir != "1" && dir != "2" && dir != "3")
                return "NO";
            if (Move(guid, Direction.Up + int.Parse(dir)))
                return "OK";
            else
                return "NO";
        }

        string DecideTake(string guid)
        {
            if (TakePiece(guid))
                return "T";
            else
                return "F";
        }

        string DecideDiscover(string guid)
        {
            Position playerPosition = FindPlayer(guid);
            if (playerPosition == null)
                return "NO";
            else
            {
                List<int> list = board.ManhattanDistance(playerPosition);
                string distances = "";
                foreach(int elem in list)
                {
                    distances += (elem.ToString() + ",");
                }
                if (distances.Split(",").Length != 9)
                    return "NO";
                else
                    return distances;
            }
        }

        string DecidePlace(string guid)
        {
            if (PlacePiece(guid))
                return "T";
            else
                return "F";
        }

        void StartPlayer()
        {
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = "GamePlayer.exe";
            psi.Arguments = new Player(1, new Team(),true).ReturnPath();
            psi.UseShellExecute = true;
            Process.Start(psi);
        }
        #endregion
        private void listen()
        {

        }

        public GameMasterConfiguration LoadConfigurationFromJSON(string path)
        {
            return null;
        }

        public void SaveConfigurationToJSON(string path)
        {

        }

        private void PutNewPiece()
        {

        }

        private void PrintBoard()
        {

        }

        public void MessageHandler(string message)
        {

        }

        public bool TakePiece(string playerGUID)
        {

            foreach (var elem in board.cellsGrid)
            {
                if(elem.GetPlayerGuid()==playerGUID)
                {
                    if(elem.GetCellState() == CellState.Piece)
                    {
                        elem.SetCellState(CellState.Empty);
                        return true;
                    }
                    else
                    {
                        elem.SetCellState(CellState.Empty);
                        return false;
                    }
                }
            }
            return false;
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

        public bool Move(string playerGUID,Direction direction)
        {
            Position playerPosition = FindPlayer(playerGUID);
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
                            board.GetCell(destinationPosition).SetPlayerGuid(playerGUID);
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
                            board.GetCell(destinationPosition).SetPlayerGuid(playerGUID);
                            return true;
                        }

                    }
                    break;
            }

            return false;
        }

        public bool PlacePiece(string playerGUID)
        {
            Position playerPosition = FindPlayer(playerGUID);

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


        public void processMessage(string message)
        {
            string[] messageSplit = message.Split("_");
            string playerGUID = messageSplit[0];
            ActionType action = (ActionType)int.Parse(messageSplit[1]);

            switch(action)
            {
                case ActionType.Move:
                        Direction direction= (Direction)int.Parse(messageSplit[2]);
                        Move(playerGUID, direction);
                        break;
                case ActionType.Pickup:
                    TakePiece(playerGUID);
                    break;
                case ActionType.Place:
                    PlacePiece(playerGUID);
                    break;

            }



        }


              private string ReceiveFromPlayer()   
        {
            using (NamedPipeServerStream pipeServer =
            new NamedPipeServerStream("GM_Player_Server", PipeDirection.In))
            {
                pipeServer.WaitForConnection();

                using (StreamReader sr = new StreamReader(pipeServer))
                {
                    string temp;
                    while ((temp = sr.ReadLine()) != null)
                    {
                        Console.WriteLine("Received from server: {0}", temp);
                        return temp;
                    }
                    return null;
                }
            }
        }

        public void SendToPlayer(string message, string guid)
        {
            using (NamedPipeClientStream pipeClient =
            new NamedPipeClientStream(".", "Player_Pipe_Server"+guid, PipeDirection.Out))
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


        public void SendToGUI() { }

        public string MessageOptionsForGUI()
        {
            string message = "o;";

            message += "w," + board.boardWidth.ToString() + ";";
            message += "h," + board.boardHeight.ToString() + ";";
            message += "g," + board.goalAreaHeight.ToString() + ";";
            message += "t," + board.taskAreaHeight.ToString() + ";";

            if(teamRedGuids != null)
                message += "r," + teamRedGuids.Count.ToString() + ";";
            else
                message += "r,0;";

            if (teamBlueGuids != null)
                message += "b," + teamBlueGuids.Count.ToString() + ";";
            else
                message += "b,0;";

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
                        if (board.cellsGrid[i, j].GetPlayerGuid() == teamRedGuids[k])
                        {
                            message += "7,r," + board.cellsGrid[i, j].GetPlayerGuid() + ",";
                            flag = true;
                        }
                    }
                    for (int k = 0; teamBlueGuids != null && k < teamBlueGuids.Count && !flag; k++)
                    {
                        if (board.cellsGrid[i, j].GetPlayerGuid() == teamBlueGuids[k])
                        {
                            message += "7,b," + board.cellsGrid[i, j].GetPlayerGuid() + ",";
                            flag = true;
                        }
                    }
                    if(!flag)
                        message += ((int)(board.cellsGrid[i, j].GetCellState())).ToString() + ",";
                }
            }

            message = message.Substring(0, message.Length - 1);
            message += ";";

            return message;
        }

        public string MessageEndForGUI() 
        {
            string message = "e;";

            message += "End;";

            return message;
        }

    }
}
