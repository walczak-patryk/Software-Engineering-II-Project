using GameGraphicalInterface;
using GameMaster.Boards;
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
        private List<Guid> teamRedGuids;
        private List<Guid> teamBlueGuids;

        private Process GuiWindow;


        public void StartGame()
        {
            this.configuration = new GameMasterConfiguration();
            this.board = new GameMasterBoard(this.configuration.boardGoalHeight, this.configuration.boardGoalHeight, this.configuration.boardTaskHeight);
            this.status = GameMasterStatus.Active;
            teamBlueGuids = new List<Guid> { new Guid(), new Guid(), new Guid() };
            teamRedGuids = new List<Guid> { new Guid(), new Guid(), new Guid() };
            Task.Run(() =>
            {
                while (true)
                    this.ReceiveFromGUI();
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
            psi.Arguments = new MainWindow(board, configuration.shamProbability, configuration.maxPieces, configuration.initialPieces, configuration.predefinedGoalPositions).ReturnPath();
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
            psi.Arguments = new Player(1,"1", new Team(),true).ReturnPath();
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
    }
}
