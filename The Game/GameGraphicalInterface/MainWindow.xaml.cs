using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using GameMaster.Boards;
using System.Windows.Threading;
using System.IO.Pipes;
using System.IO;
using GameMaster.Cells;

namespace GameGraphicalInterface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public GameMasterBoard GMboard;
        DispatcherTimer MsgTimer;
        string GMmsg;
        bool newMsg;

        private bool isPlaying;

        public int redScore;
        public int blueScore;

        ////To be changed to GameMasterConfiguration and GameMaster
        //public double shamChance;
        //public int maxPieces;
        //int initPieces;
        //System.Drawing.Point[] goals;
        //public List<Player> players;
        //List<PlayerWindow> playerWs;
        ////=======
        //bool generated;

        public MainWindow()
        {
            InitializeComponent();
            GMboard = null;
            GMmsg = "";
            isPlaying = false;

            var t1 = Task.Run(() =>
            {
                while (true)
                {
                    this.ReceiveFromGM();
                    if (null == GMboard && newMsg)
                        ParseMessageFromGM();
                }
            });

            StartSendingFromGM();

            //test
            //this.GMboard = new GameMasterBoard(10, 3, 12);
            //for (int j = 0; j < this.GMboard.boardHeight; j++)
            //{
            //    for (int i = 0; i < this.GMboard.boardWidth; i++)
            //    {
            //        this.GMboard.cellsGrid[i, j] = new Cell(0);
            //    }
            //}
            //this.GMboard.cellsGrid[3, 3].SetPlayerGuid("r1");
            //this.GMboard.cellsGrid[3, 7].SetPlayerGuid("b2");

            //shamChance = 0.5;
            //maxPieces = 2;
            //initPieces = 3;
            //goals = new System.Drawing.Point[] { new System.Drawing.Point(3, 0), new System.Drawing.Point(3, 2) };
            //players = new List<Player>();
            //generated = false;
            //redScore = 0;
            //blueScore = 0;
            //playerWs = new List<PlayerWindow>();
        }

        private void StartPrinting(object sender, RoutedEventArgs e)
        {
            if (!isPlaying)
                return;
            Welcome.Width = new GridLength(0, GridUnitType.Pixel);
            Board.Width = new GridLength(1, GridUnitType.Star);
            this.PrintBoard();
            if (MsgTimer == null)
            {
                MsgTimer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromMilliseconds(33)
                };
                MsgTimer.Tick += Printing;
                MsgTimer.Start();
            }
        }

        private void Printing(object sender, EventArgs e)
        {
            if (isPlaying && newMsg)
            {
                this.ParseMessageFromGM();
                this.PrintBoard();
                newMsg = false;
            }
        }

        #region GM Communication
        public string ReturnPath()
        {
            return Environment.CurrentDirectory;
        }

        private void SendToGM(string message)
        {
            using NamedPipeClientStream pipeClient =
            new NamedPipeClientStream(".", "GM_Pipe_Server", PipeDirection.Out);
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
                MessageBox.Show("ERROR: {0}", e.Message);
            }
        }

        private void ReceiveFromGM()
        {
            string res = "";
            using (NamedPipeServerStream pipeServer =
            new NamedPipeServerStream("GUI_Pipe_Server", PipeDirection.In))
            {
                pipeServer.WaitForConnection();
                using StreamReader sr = new StreamReader(pipeServer);
                string temp;
                while ((temp = sr.ReadLine()) != null)
                {
                    res = temp;
                    MessageBox.Show("Received from server: " + temp, "Message");
                }
            }
            GMmsg = res;
            newMsg = true;
            return;
        }
        #endregion

        private void StartSendingFromGM()
        {
            this.SendToGM("1_1");
        }

        private void PrintBoard()
        {
            if (GMboard == null) return;

            if(panel.Children.Count == 0)
            {
                panel.Children.Clear(); 

                for (int i = 0; i < GMboard.boardHeight; i++)
                {
                    StackPanel stkp = new StackPanel
                    {
                        Orientation = Orientation.Horizontal,
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Stretch
                    };
                    panel.Children.Add(stkp);

                    for (int j = 0; j < GMboard.boardWidth; j++)
                    {
                        TextBox txtb = new TextBox
                        {
                            MinHeight = 30,
                            Height = (this.Height - 50) / GMboard.boardHeight,
                            MinWidth = 30,
                            Width = (this.Width - 50) / GMboard.boardWidth,
                            TextAlignment = TextAlignment.Center,
                            IsReadOnly = true
                        };
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

                    if (i < GMboard.goalAreaHeight || i >= GMboard.taskAreaHeight + GMboard.goalAreaHeight)
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

                    if (GMboard.cellsGrid[j, i].GetCellState() == GameMaster.Cells.CellState.Piece)
                    {
                        txtb.Text = "P";
                        txtb.Background = Brushes.Black;
                        txtb.Foreground = Brushes.White;
                    }

                    if (GMboard.cellsGrid[j, i].GetPlayerGuid() != null)
                    {
                        var guid = GMboard.cellsGrid[j, i].GetPlayerGuid();
                        if (guid[0]=='r')
                            txtb.Background = Brushes.Red;
                        else if (guid[0] == 'b')
                            txtb.Background = Brushes.Blue;
                        txtb.Foreground = Brushes.White;
                        txtb.Text = guid.Remove(0,1);
                    }
                }
            }

        }

        //private void ParseMessageFromGM()
        //{
        //    foreach (var i in playerWs)
        //        i.Close();
        //}

        private void ParseMessageFromGM()
        {
            string[] parts = GMmsg.Split(";");
            if ("o" == parts[0])
            {
                int width = -1, goalHeight = -1, taskHeight = -1;
                
                for(int i = 1; i < parts.Length; i++) {
                    string[] option = parts[i].Split(",");
                    switch(option[0])
                    {
                        case "w":
                            width = int.Parse(option[1]);
                            break;
                        case "g":
                            goalHeight = int.Parse(option[1]);
                            break;
                        case "t":
                            taskHeight = int.Parse(option[1]);
                            break;
                        default:
                            break;
                    }
                }
                if (width > 0 && goalHeight > 0 && taskHeight > 0)
                {
                    this.GMboard = new GameMasterBoard(width, goalHeight, taskHeight);
                    for (int j = 0; j < this.GMboard.boardHeight; j++)
                    {
                        for (int i = 0; i < this.GMboard.boardWidth; i++)
                        {
                            this.GMboard.cellsGrid[i, j] = new Cell(0);
                        }
                    }
                }
                isPlaying = true;                  
            }
            else if (isPlaying && "s" == parts[0])
            {
                string[] update = parts[1].Split(",");

                int indx = 0;

                while (indx < update.Length)
                {
                    for (int j = 0; j < GMboard.boardHeight; j++)
                    {
                        for (int i = 0; i < this.GMboard.boardWidth; i++)
                        {
                            this.GMboard.cellsGrid[i, j].SetPlayerGuid(null);
                            switch (update[indx])
                            {
                                case "0":
                                    this.GMboard.cellsGrid[i, j].SetCellState(CellState.Empty);
                                    break;
                                case "1":
                                    this.GMboard.cellsGrid[i, j].SetCellState(CellState.Goal);
                                    break;
                                case "2":
                                    this.GMboard.cellsGrid[i, j].SetCellState(CellState.Piece);
                                    break;
                                case "4":
                                    this.GMboard.cellsGrid[i, j].SetCellState(CellState.Valid);
                                    break;
                                case "5":
                                    this.GMboard.cellsGrid[i, j].SetCellState(CellState.Unknown);
                                    break;
                                case "6":
                                    this.GMboard.cellsGrid[i, j].SetCellState(CellState.NoGoal);
                                    break;
                                case "7":
                                    this.GMboard.cellsGrid[i, j].SetCellState(CellState.Empty);
                                    this.GMboard.cellsGrid[i, j].SetPlayerGuid(update[indx + 1] + update[indx + 2]);
                                    indx += 2;
                                    break;
                            }
                            indx++;
                        }
                    }
                }
            }
            else if (isPlaying && "e" == parts[0])
            {
                // Message with final stats, end game.
                isPlaying = false;
            }
            else
                throw new Exception("Wrong message.");
        }
    }

    #region nie uzywane
    //private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    //{
    //    foreach (var i in playerWs)
    //        i.Close();
    //}

    //public MainWindow(GameMasterBoard gmboard, double shamProb, int maxPieces, int initPieces, System.Drawing.Point[] goals)
    //{
    //    InitializeComponent();
    //    GMboard = gmboard;
    //    shamChance = shamProb;
    //    this.maxPieces = maxPieces;
    //    this.initPieces = initPieces;
    //    this.goals = goals;
    //    players = new List<Player>();
    //    generated = false;
    //    GMmsg = "";
    //    var t = Task.Run(() =>
    //    {
    //        while (true)
    //            this.ReceiveFromGM();
    //    });
    //}

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



    //private void StartServer()
    //{
    //    throw new NotImplementedException();
    //}

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
    #endregion
}
