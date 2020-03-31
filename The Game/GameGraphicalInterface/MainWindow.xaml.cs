using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GameMaster.Positions;
using GameMaster.Boards;
using System.Timers;
using System.Windows.Threading;

namespace GameGraphicalInterface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        GameMasterBoard GMboard;
        DispatcherTimer PTimer;
        double shamChance;
        int maxPieces;
        int initPieces;
        System.Drawing.Point[] goals;
        public MainWindow()
        {
            InitializeComponent();
            GMboard = null;
            shamChance = 0.5;
            maxPieces = 2;
            initPieces = 3;
            goals = new System.Drawing.Point[] { new System.Drawing.Point(3, 0), new System.Drawing.Point(3, 2) };
        }

        public MainWindow(GameMasterBoard gmboard, double shamProb, int maxPieces, int initPieces, System.Drawing.Point[] goals)
        {
            InitializeComponent();
            GMboard = gmboard;
            shamChance = shamProb;
            this.maxPieces = maxPieces;
            this.initPieces = initPieces;
            this.goals = goals;
        }

        public string ReturnPath()
        {
            return Environment.CurrentDirectory;
        }

        private void AddGoal(object sender, RoutedEventArgs e)
        {
            int x = Int32.Parse(gxBox.Text);
            int y = Int32.Parse(gyBox.Text);

            if (x >= GMboard.boardWidth)
                return;
            if (y >= GMboard.goalAreaHeight)
                return;

            Position pos = new Position();
            pos.x = x;
            pos.y = y;

            Position pos2 = new Position();
            pos2.x = x;
            pos2.y = y + GMboard.taskAreaHeight + GMboard.goalAreaHeight;
            
            GMboard.SetGoal(pos);
            GMboard.SetGoal(pos2);
        }

        private void GenerateBoard(object sender, RoutedEventArgs e)
        {
            if (GMboard == null)
                GMboard = new GameMasterBoard(Int32.Parse(bwidthBox.Text), Int32.Parse(goalhBox.Text), Int32.Parse(taskhBox.Text));

            bwidthBox.IsReadOnly = true;
            goalhBox.IsReadOnly = true;
            taskhBox.IsReadOnly = true;

            if(PTimer == null)
            {
                PTimer = new DispatcherTimer();
                PTimer.Interval = TimeSpan.FromSeconds(30);
                PTimer.Tick += OnTimedEvent;
                PTimer.Start();
            }

            if (goals != null)
            {
                foreach (var i in goals)
                {
                    GMboard.SetGoal(new Position() { x = i.X, y = i.Y });
                    GMboard.SetGoal(new Position() { x = i.X, y = i.Y + GMboard.taskAreaHeight + GMboard.goalAreaHeight });
                }
            }

            for (int i = 0; i < initPieces; i++)
                GMboard.generatePiece(shamChance, initPieces);
        }

        private void OnTimedEvent(object sender, EventArgs e)
        {
            for(int i = 0; i < maxPieces; i++)
                GMboard.generatePiece(shamChance, maxPieces);
            PrintBoard(this, e as RoutedEventArgs);
        }

        private void PrintBoard(object sender, RoutedEventArgs e)
        {
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

                        if (i < GMboard.goalAreaHeight || i >= GMboard.goalAreaHeight + GMboard.taskAreaHeight)
                        {
                            txtb.BorderBrush = new SolidColorBrush(Colors.Black);
                            txtb.BorderThickness = new Thickness(1);
                        }
                        if (GMboard.cellsGrid[j, i].GetCellState() == GameMaster.Cells.CellState.Goal)
                        {
                            txtb.Text = "G";
                            txtb.Background = Brushes.LightYellow;
                        }
                        if (GMboard.cellsGrid[j, i].GetCellState() == GameMaster.Cells.CellState.Piece || GMboard.cellsGrid[j, i].GetCellState() == GameMaster.Cells.CellState.Sham)
                        {
                            txtb.Text = "P";
                            txtb.Background = Brushes.Black;
                            txtb.Foreground = Brushes.White;
                        }

                        stkp.Children.Add(txtb);
                    }
                }
            }
            else
            {
                UIElementCollection panels = panel.Children;
                for (int i = 0; i < GMboard.boardHeight; i++)
                {
                    StackPanel stkp = panels[i] as StackPanel;
                    for (int j = 0; j < GMboard.boardWidth; j++)
                    {
                        TextBox txtb = stkp.Children[j] as TextBox;

                        if (i < GMboard.goalAreaHeight || i >= GMboard.goalAreaHeight + GMboard.taskAreaHeight)
                        {
                            txtb.BorderBrush = new SolidColorBrush(Colors.Black);
                            txtb.BorderThickness = new Thickness(1);
                        }

                        if (GMboard.cellsGrid[j, i].GetCellState() == GameMaster.Cells.CellState.Goal)
                        {
                            txtb.Text = "G";
                            txtb.Background = Brushes.LightYellow;
                        }
                        else if (GMboard.cellsGrid[j, i].GetCellState() == GameMaster.Cells.CellState.Piece || GMboard.cellsGrid[j, i].GetCellState() == GameMaster.Cells.CellState.Sham)
                        {
                            txtb.Text = "P";
                            txtb.Background = Brushes.Black;
                            txtb.Foreground = Brushes.White;
                        }
                        else
                        {
                            txtb.Text = "";
                            txtb.Background = Brushes.White;
                            txtb.Foreground = Brushes.Black;
                        }
                    }
                }
            }
            
        }

        private void CreatePlayer(object sender, RoutedEventArgs e)
        {
            string pName = plName.Text;
            string tStr = plTeam.Text;

            GameMaster.Team team = new GameMaster.Team();
            if (tStr == "Red")
                team.SetColor(GameMaster.TeamColor.Red);
            else
                team.SetColor(GameMaster.TeamColor.Blue);

            PlayerWindow playerWindow = new PlayerWindow(this, pName, team, GMboard.boardWidth, GMboard.goalAreaHeight, GMboard.taskAreaHeight);
            playerWindow.Show();
        }
    }
}
