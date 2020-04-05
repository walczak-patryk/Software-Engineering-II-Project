using GameGraphicalInterface;
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
        private GameMasterBoard board;
        private GameMasterStatus status;
        private GameMasterConfiguration configuration;
        private List<string> teamRedGuids;
        private List<string> teamBlueGuids;

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
                    this.SendToGUI("0_1");
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

            //Cell cell = new Cell();
            foreach (var elem in board.cellsGrid)
            {
                if(elem.GetPlayerGuid()==playerGUID)
                {
                    elem.SetCellState(CellState.Empty);
                    break;
                }
            }

            return true;
        }

        public bool Move(string playerGUID,Direction direction)
        {
            int playerX = -1;
            int playerY = -1;
            bool playerFound = false;
            for(int x=0;x<board.boardWidth;x++)
            {
                for(int y=0; y<board.boardHeight;y++)
                {
                    if(board.cellsGrid[x,y].GetPlayerGuid()==playerGUID)
                    {
                        playerX = x;
                        playerY = y;
                        playerFound = true;
                        break;
                    }
                }
                if(playerFound)
                {
                    break;
                }
            }



            int destinationX = playerX;
            int destinationY = playerY;
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

            Position playerPosition = new Position(playerX, playerY);
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
            int playerX = -1;
            int playerY = -1;
            bool playerFound = false;
            for (int x = 0; x < board.boardWidth; x++)
            {
                for (int y = 0; y < board.boardHeight; y++)
                {
                    if (board.cellsGrid[x, y].GetPlayerGuid() == playerGUID)
                    {
                        playerX = x;
                        playerY = y;
                        playerFound = true;
                        break;
                    }
                }
                if (playerFound)
                {
                    break;
                }
            }

            TeamColor teamColor;
            if (teamRedGuids.Contains(playerGUID))
            {
                teamColor = TeamColor.Red;
            }
            else
            {
                teamColor = TeamColor.Blue;
            }
            Position position = new Position(playerX, playerY);
            if ((teamColor == TeamColor.Red && playerX < board.goalAreaHeight) ||
                (teamColor == TeamColor.Blue && playerY >= board.boardHeight - board.goalAreaHeight))
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


              private void ReceiveFromPlayer()   
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
                    }
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




    }
}
