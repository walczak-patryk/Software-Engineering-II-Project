using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using GameMaster.Positions;
using GameMaster.Boards;
using System.Windows.Threading;
using GameMaster;
using System.IO.Pipes;
using System.IO;
using System.Threading;
using GameMaster.Cells;

namespace GameGraphicalInterface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public GameMasterBoard GMboard;
        DispatcherTimer PTimer;
        DispatcherTimer PrintTimer;
        DispatcherTimer MsgTimer;
        //To be changed to GameMasterConfiguration and GameMaster
        public double shamChance;
        public int maxPieces;
        int initPieces;
        System.Drawing.Point[] goals;
        public List<Player> players;
        List<PlayerWindow> playerWs;
        //=======
        bool generated;
        string GMmsg;
        bool newMsg;
        public int redScore;
        public int blueScore;
        public MainWindow()
        {
            InitializeComponent();
            GMboard = null;
            shamChance = 0.5;
            maxPieces = 2;
            initPieces = 3;
            goals = new System.Drawing.Point[] { new System.Drawing.Point(3, 0), new System.Drawing.Point(3, 2) };
            players = new List<Player>();
            generated = false;
            GMmsg = "";
            var t1 = Task.Run(() =>
            {
                while (true)
                    this.ReceiveFromGM();
            });
            redScore = 0;
            blueScore = 0;
            playerWs = new List<PlayerWindow>();
        }

        private void StartPrinting(object sender, RoutedEventArgs e)
        {
            Welcome.Width = new GridLength(0, GridUnitType.Pixel);
            Board.Width = new GridLength(150, GridUnitType.Pixel);
            if (MsgTimer == null)
            {
                MsgTimer = new DispatcherTimer();
                MsgTimer.Interval = TimeSpan.FromSeconds(1);
                MsgTimer.Tick += Printing;
                MsgTimer.Start();
            }
        }

        private void Printing(object sender, EventArgs e)
        {
            if (newMsg)
            {
                switch (GMmsg)
                {
                    case "msg generate board":
                        //GenerateBoard(this);
                        break;
                    case "msg print board":
                        PrintBoard(this);
                        break;
                }
                newMsg = false;
            }
        }

        public MainWindow(GameMasterBoard gmboard, double shamProb, int maxPieces, int initPieces, System.Drawing.Point[] goals)
        {
            InitializeComponent();
            GMboard = gmboard;
            shamChance = shamProb;
            this.maxPieces = maxPieces;
            this.initPieces = initPieces;
            this.goals = goals;
            players = new List<Player>();
            generated = false;
            GMmsg = "";
            var t = Task.Run(() =>
            {
                while (true)
                    this.ReceiveFromGM();
            });
        }

        #region GM Communication
        public string ReturnPath()
        {
            return Environment.CurrentDirectory;
        }

        private void SendToGM(string message)
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
                    MessageBox.Show("ERROR: {0}", e.Message);
                }
            }
        }

        private void ReceiveFromGM()
        {
            string res = "";
            using (NamedPipeServerStream pipeServer =
            new NamedPipeServerStream("GUI_Pipe_Server", PipeDirection.In))
            {
                pipeServer.WaitForConnection();
                using (StreamReader sr = new StreamReader(pipeServer))
                {
                    string temp;
                    while ((temp = sr.ReadLine()) != null)
                    {
                        res = temp;
                        MessageBox.Show("Received from server: {0}", temp);
                    }
                }
            }
            GMmsg = res;
            newMsg = true;
            return;
        }

        private void Send(object sender, RoutedEventArgs e)
        {
            SendToGM("nacisnieto przycisk send");
        }
        #endregion


        //private void AddGoal(object sender, RoutedEventArgs e)
        //{
        //    int x = Int32.Parse(gxBox.Text);
        //    int y = Int32.Parse(gyBox.Text);

        //    if (x >= GMboard.boardWidth)
        //        return;
        //    if (y >= GMboard.goalAreaHeight)
        //        return;

        //    Position pos = new Position();
        //    pos.x = x;
        //    pos.y = y;

        //    Position pos2 = new Position();
        //    pos2.x = x;
        //    pos2.y = y + GMboard.taskAreaHeight + GMboard.goalAreaHeight;
            
        //    GMboard.SetGoal(pos);
        //    GMboard.SetGoal(pos2);          
        //}

        //private void GenerateBoard(object sender)
        //{
        //    try
        //    {
        //        if (GMboard == null)
        //            GMboard = new GameMasterBoard(Int32.Parse(bwidthBox.Text), Int32.Parse(goalhBox.Text), Int32.Parse(taskhBox.Text));

        //        bwidthBox.IsReadOnly = true;
        //        goalhBox.IsReadOnly = true;
        //        taskhBox.IsReadOnly = true;

        //        if (PTimer == null)
        //        {
        //            PTimer = new DispatcherTimer();
        //            PTimer.Interval = TimeSpan.FromSeconds(30);
        //            PTimer.Tick += OnTimedEvent;
        //            PTimer.Start();
        //        }

        //        if (PrintTimer == null)
        //        {
        //            PrintTimer = new DispatcherTimer();
        //            PrintTimer.Interval = TimeSpan.FromSeconds(1);
        //            PrintTimer.Tick += OnTimedEvent2;
        //            PrintTimer.Start();
        //        }

        //        if (goals != null)
        //        {
        //            foreach (var i in goals)
        //            {
        //                GMboard.SetGoal(new Position(i.X, i.Y));
        //                GMboard.SetGoal(new Position(i.X, i.Y + GMboard.taskAreaHeight + GMboard.goalAreaHeight));
        //            }
        //        }

        //        for (int i = 0; i < maxPieces; i++)
        //            GMboard.generatePiece(shamChance, maxPieces);

        //        generated = true;
        //    }
        //    catch(Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
            
        //}

        //private void OnTimedEvent(object sender, EventArgs e)
        //{
        //    int pi = GMboard.piecesPositions.Count();
        //    for(int i = pi; i < maxPieces; i++)
        //        GMboard.generatePiece(shamChance, maxPieces);
        //    PrintBoard(this, e as RoutedEventArgs);
        //    foreach(var i in players)
        //    {
        //        for(int j = 0; j < GMboard.boardWidth; j++)
        //        {
        //            for (int k = 0; k < GMboard.boardHeight; k++)
        //                //i.board.cellsGrid[j, k].SetDistance(Math.Max(GMboard.boardWidth, GMboard.boardHeight));
        //                i.board.cellsGrid[j, k] = GMboard.cellsGrid[j, k].Copy();
        //        }
        //    }
        //}

        //private void OnTimedEvent2(object sender, EventArgs e)
        //{
        //    PrintBoard(this, e as RoutedEventArgs);
        //    if (redScore == goals.Length)
        //        Display.Content = "Read Team has Won!!!";
        //    if (blueScore == goals.Length)
        //        Display.Content = "Blue Team has Won!!!";
        //}

        private void PrintBoard(object sender)
        {
            if (!generated)
                return;

            if(panel.Children.Count == 0)
            {
                panel.Children.Clear();

                for (int i = 0; i < GMboard.boardHeight; i++)
                {
                    StackPanel stkp = new StackPanel();
                    stkp.Orientation = Orientation.Horizontal;
                    stkp.Width = 30 * GMboard.boardWidth;
                    stkp.Height = 30;
                    stkp.HorizontalAlignment = HorizontalAlignment.Stretch;
                    stkp.VerticalAlignment = VerticalAlignment.Stretch;
                    panel.Children.Add(stkp);

                    for (int j = 0; j < GMboard.boardWidth; j++)
                    {
                        TextBox txtb = new TextBox();
                        txtb.Height = 30;
                        txtb.Width = 30;
                        txtb.TextAlignment = TextAlignment.Center;
                        txtb.IsReadOnly = true;
                        stkp.Children.Add(txtb);
                    }
                }
            }

            UIElementCollection panels = panel.Children;
            for (int i = 0; i < GMboard.boardHeight; i++)
            {
                StackPanel stkp = panels[i] as StackPanel;
                for (int j = 0; j < GMboard.boardWidth; j++)
                {
                    TextBox txtb = stkp.Children[j] as TextBox;

                    txtb.Text = "";
                    txtb.Background = Brushes.White;
                    txtb.Foreground = Brushes.Black;

                    if (i < GMboard.goalAreaHeight || i >= GMboard.goalAreaHeight + GMboard.taskAreaHeight)
                    {
                        txtb.BorderBrush = new SolidColorBrush(Colors.Black);
                        txtb.BorderThickness = new Thickness(1);
                    }

                    if (GMboard.cellsGrid[j, i].GetCellState() == GameMaster.Cells.CellState.Valid)
                    {
                        txtb.Text = "G";
                        txtb.Background = Brushes.LightYellow;
                    }

                    if (GMboard.cellsGrid[j, i].GetCellState() == GameMaster.Cells.CellState.Goal)
                    {
                        txtb.Text = "YG";
                        txtb.Background = Brushes.LightYellow;
                    }

                    if (GMboard.cellsGrid[j, i].GetCellState() == GameMaster.Cells.CellState.NoGoal)
                    {
                        txtb.Text = "NG";
                        txtb.Background = Brushes.White;
                    }

                    if (GMboard.cellsGrid[j, i].GetCellState() == GameMaster.Cells.CellState.Piece || GMboard.cellsGrid[j, i].GetCellState() == GameMaster.Cells.CellState.Sham)
                    {
                        txtb.Text = "P";
                        txtb.Background = Brushes.Black;
                        txtb.Foreground = Brushes.White;
                    }
                    
                    if (GMboard.cellsGrid[j, i].GetPlayerGuid() != null)
                    {
                        Player p = players.Find(x => x.guid == GMboard.cellsGrid[j, i].GetPlayerGuid());
                        if (p.team.getColor() == TeamColor.Red)
                            txtb.Background = Brushes.Red;
                        else
                            txtb.Background = Brushes.Blue;
                        txtb.Foreground = Brushes.White;
                        txtb.Text = GMboard.cellsGrid[j, i].GetPlayerGuid();
                    }
                }
            }

        }

        //private void CreatePlayer(object sender, RoutedEventArgs e)
        //{
        //    string pName = plName.Text;
        //    string tStr = plTeam.Text;
        //    string strat = StratCb.Text;

        //    Team team = new Team();
        //    if (tStr == "Red")
        //        team.SetColor(TeamColor.Red);
        //    else
        //        team.SetColor(TeamColor.Blue);

        //    PlayerWindow playerWindow = new PlayerWindow(this, pName, team, strat);
        //    playerWs.Add(playerWindow);
        //    playerWindow.Show();
        //}



        private void StartServer()
        {
            throw new NotImplementedException();
        }

        //private void Listen(object sender, RoutedEventArgs e)
        //{
        //    if (MsgTimer == null)
        //    {
        //        MsgTimer = new DispatcherTimer();
        //        MsgTimer.Interval = TimeSpan.FromSeconds(1);
        //        MsgTimer.Tick += OnTimedEvent3;
        //        MsgTimer.Start();
        //    }
        //    //plName.Text = GMmsg;
        //}

        //private void OnTimedEvent3(object sender, EventArgs e)
        //{
        //    if (GMmsg != "")
        //    {
        //        if (GMmsg == "msg generate board")
        //            GenerateBoard(this, e as RoutedEventArgs);
        //        if (GMmsg == "msg print board")
        //            PrintBoard(this, e as RoutedEventArgs);
        //        GMmsg = "";
        //    }
        //}

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            foreach (var i in playerWs)
                i.Close();
        }

        private void ParseMessageFromGUI(string message)
        {
            string[] parts = message.Split(";");
            if ("o" == parts[0])
            {
                int width, height, goalHeight, taskHeight, red, blue;
                for(int i = 1; i < parts.Length; i++) {
                    string[] option = parts[i].Split(",");
                    switch(option[0])
                    {
                        case "w":
                            width = int.Parse(option[1]);
                            break;
                        case "h":
                            height = int.Parse(option[1]);
                            break;
                        case "g":
                            goalHeight = int.Parse(option[1]);
                            break;
                        case "t":
                            taskHeight = int.Parse(option[1]);
                            break;
                        case "r":
                            red = int.Parse(option[1]);
                            break;
                        case "b":
                            blue = int.Parse(option[1]);
                            break;
                        default:
                            break;
                    }
                }
            }
            else if ("s" == parts[0])
            {
                int width = GMboard.boardWidth;
                int height = GMboard.boardHeight;
                Cell[,] cells = new Cell[width, height];

                string[] update = parts[1].Split(",");

                List<string> players = new List<string>();

                int indx = 0;

                while (indx < update.Length)
                {
                    for (int j = 0; j < height; j++)
                    {
                        for (int i = 0; i < width; i++)
                        {
                            switch (update[indx])
                            {
                                case "0":
                                    cells[i, j] = new Cell(0);
                                    cells[i, j].SetCellState(CellState.Empty);
                                    break;
                                case "1":
                                    cells[i, j] = new Cell(0);
                                    cells[i, j].SetCellState(CellState.Goal);
                                    break;
                                case "2":
                                    cells[i, j] = new Cell(0);
                                    cells[i, j].SetCellState(CellState.Piece);
                                    break;
                                case "3":
                                    cells[i, j] = new Cell(0);
                                    cells[i, j].SetCellState(CellState.Sham);
                                    break;
                                case "4":
                                    cells[i, j] = new Cell(0);
                                    cells[i, j].SetCellState(CellState.Valid);
                                    break;
                                case "5":
                                    cells[i, j] = new Cell(0);
                                    cells[i, j].SetCellState(CellState.Unknown);
                                    break;
                                case "6":
                                    cells[i, j] = new Cell(0);
                                    cells[i, j].SetCellState(CellState.NoGoal);
                                    break;
                                case "7":
                                    cells[i, j] = new Cell(0);
                                    cells[i, j].SetCellState(CellState.Empty);
                                    string p = i.ToString() + "," + j.ToString() + "," + update[++indx] + "," + update[++indx];
                                    players.Add(p);
                                    break;
                            }
                            indx++;
                        }
                    }
                }
            }
            else if ("e" == parts[0])
            {
                // Message with final stats, end game.
            }
            else
                throw new Exception("No option symbol in the message.");
        }
    }
}
