using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using GameMaster;
using GameMaster.Boards;
using GameMaster.Positions;
using System.Windows.Threading;

namespace GameGraphicalInterface
{
    /// <summary>
    /// Interaction logic for PlayerWindow.xaml
    /// </summary>
    public partial class PlayerWindow : Window
    {
        MainWindow main;
        Player player;
        Board playerBoard;
        DispatcherTimer PrintTimer;
        string strategy;
        bool justDiscovered;
        bool tested;
        public PlayerWindow(MainWindow wind, string name, Team team, string strat)
        {
            main = wind;
            strategy = strat;
            justDiscovered = false;
            tested = false;
            Random rand = new Random();
            player = new Player(rand.Next() % 50 , team, false);
            InitializeComponent();
            SetInfo(name, team);
            playerBoard = new Board(main.GMboard.boardWidth, main.GMboard.goalAreaHeight, main.GMboard.taskAreaHeight);
            player.board = playerBoard;
            for (int j = 0; j < player.board.boardWidth; j++)
            {
                for (int k = 0; k < player.board.boardHeight; k++)
                {
                    //player.board.cellsGrid[j, k].SetCellState(main.GMboard.cellsGrid[j, k].GetCellState());
                    //player.board.cellsGrid[j, k].SetDistance(main.GMboard.cellsGrid[j, k].GetDistance());
                    //player.board.cellsGrid[j, k].SetPlayerGuid(main.GMboard.cellsGrid[j, k].GetPlayerGuid());
                    player.board.cellsGrid[j, k] = main.GMboard.cellsGrid[j, k].Copy();
                }
            }
            //playerBoard.cellsGrid = main.GMboard.cellsGrid;

            int x = rand.Next() % player.board.boardWidth;
            int y = rand.Next() % player.board.goalAreaHeight;
            if (team.getColor() == TeamColor.Blue)
                y += player.board.taskAreaHeight + player.board.goalAreaHeight;
            Position playerPos = new Position(x, y);
            player.position = playerPos;
            DrawPlayerBoard();

            main.GMboard.cellsGrid[x, y].SetPlayerGuid(player.guid);
            player.board.cellsGrid[x, y].SetPlayerGuid(player.guid);
            main.players.Add(player);

            PrintTimer = new DispatcherTimer();
            PrintTimer.Interval = TimeSpan.FromSeconds(1);
            PrintTimer.Tick += OnTimedEvent;
            PrintTimer.Start();
        }

        private void OnTimedEvent(object sender, EventArgs e)
        {
            DrawPlayerBoard();
            if(strategy == "AI")
            {
                AI_Move();
            }
        }

        private void DrawPlayerBoard()
        {
            if(boardPanel.Children.Count == 0)
            {
                boardPanel.Children.Clear();

                for (int i = 0; i < playerBoard.boardHeight; i++)
                {
                    StackPanel stkp = new StackPanel();
                    stkp.Orientation = Orientation.Horizontal;
                    stkp.Width = 30 * playerBoard.boardWidth;
                    stkp.Height = 30;
                    stkp.HorizontalAlignment = HorizontalAlignment.Stretch;
                    stkp.VerticalAlignment = VerticalAlignment.Stretch;
                    boardPanel.Children.Add(stkp);

                    for (int j = 0; j < playerBoard.boardWidth; j++)
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

            UIElementCollection panels = boardPanel.Children;
            for (int i = 0; i < player.board.boardHeight; i++)
            {
                StackPanel stkp = panels[i] as StackPanel;
                for (int j = 0; j < player.board.boardWidth; j++)
                {
                    TextBox txtb = stkp.Children[j] as TextBox;

                    txtb.Text = "";
                    txtb.Background = Brushes.White;
                    txtb.Foreground = Brushes.Black;

                    if (i < player.board.goalAreaHeight || i >= player.board.goalAreaHeight + player.board.taskAreaHeight)
                    {
                        txtb.BorderBrush = new SolidColorBrush(Colors.Black);
                        txtb.BorderThickness = new Thickness(1);
                    }
                    
                    if (player.board.cellsGrid[j, i].GetCellState() == GameMaster.Cells.CellState.Goal)
                    {
                        txtb.Background = Brushes.LightYellow;
                        txtb.Text = "YG";
                    }
                    
                    if (player.board.cellsGrid[j, i].GetCellState() == GameMaster.Cells.CellState.NoGoal)
                    {
                        txtb.Background = Brushes.White;
                        txtb.Text = "NG";
                    }
                    
                    if (player.board.cellsGrid[j, i].GetDistance() < Math.Max(main.GMboard.boardWidth, main.GMboard.goalAreaHeight + main.GMboard.taskAreaHeight))
                    {
                        if (player.position.x - 1 <= j && player.position.x + 1 >= j && player.position.y - 1 <= i && player.position.y + 1 >= i)
                        {
                            txtb.Text = player.board.cellsGrid[j, i].GetDistance().ToString();
                            txtb.Background = Brushes.White;
                            txtb.Foreground = Brushes.Black;
                        }
                    }

                    if (player.board.cellsGrid[j, i].GetPlayerGuid() == player.guid)
                    {
                        if (player.team.getColor() == TeamColor.Red)
                            txtb.Background = Brushes.Red;
                        else
                            txtb.Background = Brushes.Blue;
                        txtb.Foreground = Brushes.White;
                        txtb.Text = player.board.cellsGrid[j, i].GetPlayerGuid();
                    }
                }
            }
            }

        private void SetInfo(string name, Team team)
        {
            nameLabel.Content = name;
            teamLabel.Content = team.color;
        }

        private void pickButton_Click(object sender, RoutedEventArgs e)
        {
            if(player.board.cellsGrid[player.position.x, player.position.y].GetCellState() == GameMaster.Cells.CellState.Piece)
            {
                Position pos = main.GMboard.piecesPositions.Find(x => x.x == player.position.x && x.y == player.position.y);
                main.GMboard.piecesPositions.Remove(pos);

                pickButton.Visibility = Visibility.Collapsed;
                placeButton.Visibility = Visibility.Visible;
                testButton.Visibility = Visibility.Visible;

                for (int j = 0; j < player.board.boardWidth; j++)
                {
                    for (int k = 0; k < player.board.boardHeight; k++)
                        player.board.cellsGrid[j, k].SetDistance(Math.Max(player.board.boardWidth, player.board.boardHeight));
                }
                player.board.cellsGrid[player.position.x, player.position.y].SetCellState(GameMaster.Cells.CellState.Empty);
                main.GMboard.cellsGrid[player.position.x, player.position.y] = player.board.cellsGrid[player.position.x, player.position.y].Copy();
                player.piece = true;
                player.pieceIsSham = false;

                main.GMboard.generatePiece(main.shamChance, main.maxPieces);
            }
            else if (player.board.cellsGrid[player.position.x, player.position.y].GetCellState() == GameMaster.Cells.CellState.Sham)
            {
                main.GMboard.piecesPositions.Remove(player.position);

                pickButton.Visibility = Visibility.Collapsed;
                placeButton.Visibility = Visibility.Visible;
                testButton.Visibility = Visibility.Visible;

                for (int j = 0; j < player.board.boardWidth; j++)
                {
                    for (int k = 0; k < player.board.boardHeight; k++)
                        player.board.cellsGrid[j, k].SetDistance(Math.Max(player.board.boardWidth, player.board.boardHeight));
                }
                player.board.cellsGrid[player.position.x, player.position.y].SetCellState(GameMaster.Cells.CellState.Empty);
                main.GMboard.cellsGrid[player.position.x, player.position.y] = player.board.cellsGrid[player.position.x, player.position.y].Copy();
                player.piece = true;
                player.pieceIsSham = true;

                main.GMboard.generatePiece(main.shamChance, main.maxPieces);

            }
        }

        private void placeButton_Click(object sender, RoutedEventArgs e)
        {
            if (player.piece)
            {
                pickButton.Visibility = Visibility.Visible;
                placeButton.Visibility = Visibility.Hidden;
                testButton.Visibility = Visibility.Collapsed;

                player.piece = false;
                player.pieceIsSham = false;

                if(player.board.cellsGrid[player.position.x, player.position.y].GetCellState() == GameMaster.Cells.CellState.Valid)
                {
                    player.board.cellsGrid[player.position.x, player.position.y].SetCellState(GameMaster.Cells.CellState.Goal);
                    main.GMboard.cellsGrid[player.position.x, player.position.y] = player.board.cellsGrid[player.position.x, player.position.y].Copy();
                    if (player.team.getColor() == TeamColor.Red)
                        main.redScore++;
                    else
                        main.blueScore++;
                }
                else if(player.board.cellsGrid[player.position.x, player.position.y].GetCellState() == GameMaster.Cells.CellState.Empty)
                {
                    if(player.team.getColor() == TeamColor.Red && player.position.y < player.board.goalAreaHeight)
                    {
                        player.board.cellsGrid[player.position.x, player.position.y].SetCellState(GameMaster.Cells.CellState.NoGoal);
                        main.GMboard.cellsGrid[player.position.x, player.position.y] = player.board.cellsGrid[player.position.x, player.position.y].Copy();
                    }
                    else if (player.team.getColor() == TeamColor.Blue && player.position.y >= player.board.goalAreaHeight + player.board.taskAreaHeight)
                    {
                        player.board.cellsGrid[player.position.x, player.position.y].SetCellState(GameMaster.Cells.CellState.NoGoal);
                        main.GMboard.cellsGrid[player.position.x, player.position.y] = player.board.cellsGrid[player.position.x, player.position.y].Copy();
                    }
                }
            }
        }

        private void testButton_Click(object sender, RoutedEventArgs e)
        {
            if(player.piece)
            {
                if(player.pieceIsSham)
                {
                    pickButton.Visibility = Visibility.Visible;
                    placeButton.Visibility = Visibility.Hidden;
                    testButton.Visibility = Visibility.Collapsed;

                    player.piece = false;
                    player.pieceIsSham = false;
                }
            }
            else
            {

            }
        }

        private void PlayerClosed(object sender, System.ComponentModel.CancelEventArgs e)
        {
            main.players.Remove(player);
            main.GMboard.cellsGrid[player.position.x, player.position.y].SetPlayerGuid(null);
        }

        private void PlayerMove(object sender, RoutedEventArgs e)
        {
            main.GMboard.cellsGrid[player.position.x, player.position.y].SetPlayerGuid(null);
            Button butt = sender as Button;
            switch(butt.Content.ToString())
            {
                case "U":
                    {
                        player.Move(Direction.Up);
                        break;
                    }
                case "D":
                    {
                        player.Move(Direction.Down);
                        break;
                    }
                case "L":
                    {
                        player.Move(Direction.Left);
                        break;
                    }
                case "R":
                    {
                        player.Move(Direction.Right);
                        break;
                    }
                default: break;
            }
            main.GMboard.cellsGrid[player.position.x, player.position.y].SetPlayerGuid(player.guid);
            player.board.cellsGrid[player.position.x, player.position.y].SetPlayerGuid(player.guid);

        }

        private void Discover_Click(object sender, RoutedEventArgs e)
        {
            List<int> positions = main.GMboard.ManhattanDistance(player.position);
            int index = 0;
            for (int j = -1; j <= 1; j++)
            {
                for (int i = -1; i <= 1; i++)
                {
                    if (player.position.x + i < 0 || player.position.x + i >= main.GMboard.boardWidth || player.position.y + j < main.GMboard.goalAreaHeight || player.position.y + j >= main.GMboard.goalAreaHeight + main.GMboard.taskAreaHeight)
                    { index++; }
                    else
                    {
                        player.board.cellsGrid[player.position.x + i, player.position.y + j].SetDistance(positions[index]) ;
                        index++;
                    }
                }
            }
        }
        public void AI_Move()
        {
            if(!player.piece)
            {
                if (player.position.y < player.board.goalAreaHeight)
                {
                    main.GMboard.cellsGrid[player.position.x, player.position.y].SetPlayerGuid(null);
                    player.Move(Direction.Down);
                    main.GMboard.cellsGrid[player.position.x, player.position.y].SetPlayerGuid(player.guid);
                    player.board.cellsGrid[player.position.x, player.position.y].SetPlayerGuid(player.guid);
                }
                else if(!justDiscovered)
                {
                    Discover_Click(this, null);
                    justDiscovered = true;
                }
                else if(justDiscovered)
                {
                    if (player.board.cellsGrid[player.position.x, player.position.y].GetDistance() == 0)
                    {
                        pickButton_Click(this, null);
                        justDiscovered = false;
                        return;
                    }

                    int min = 100;
                    Direction dir = Direction.Down;
                    if(player.position.x > 0 && player.board.cellsGrid[player.position.x - 1, player.position.y].GetDistance() < min)
                    {
                        min = player.board.cellsGrid[player.position.x, player.position.y].GetDistance();
                        dir = Direction.Left;
                    }
                    if (player.position.x < player.board.boardWidth - 1 && player.board.cellsGrid[player.position.x + 1, player.position.y].GetDistance() < min)
                    {
                        min = player.board.cellsGrid[player.position.x, player.position.y].GetDistance();
                        dir = Direction.Right;
                    }
                    if (player.position.y > player.board.goalAreaHeight && player.board.cellsGrid[player.position.x, player.position.y - 1].GetDistance() < min)
                    {
                        min = player.board.cellsGrid[player.position.x, player.position.y].GetDistance();
                        dir = Direction.Up;
                    }
                    if (player.position.y < player.board.taskAreaHeight + player.board.goalAreaHeight && player.board.cellsGrid[player.position.x, player.position.y + 1].GetDistance() < min)
                    {
                        min = player.board.cellsGrid[player.position.x, player.position.y].GetDistance();
                        dir = Direction.Down;
                    }
                    main.GMboard.cellsGrid[player.position.x, player.position.y].SetPlayerGuid(null);
                    player.Move(dir);
                    main.GMboard.cellsGrid[player.position.x, player.position.y].SetPlayerGuid(player.guid);
                    player.board.cellsGrid[player.position.x, player.position.y].SetPlayerGuid(player.guid);
                    justDiscovered = false;
                }
            }
            else
            {
                if(!tested)
                {
                    pickButton_Click(this, null);
                    if (player.piece)
                        tested = true;
                    else
                        tested = false;
                }
                if (player.position.y > player.board.goalAreaHeight - 1 && player.team.getColor() == TeamColor.Red)
                {
                    main.GMboard.cellsGrid[player.position.x, player.position.y].SetPlayerGuid(null);
                    player.Move(Direction.Up);
                    main.GMboard.cellsGrid[player.position.x, player.position.y].SetPlayerGuid(player.guid);
                    player.board.cellsGrid[player.position.x, player.position.y].SetPlayerGuid(player.guid);
                }
                else if (player.position.y < player.board.goalAreaHeight + player.board.taskAreaHeight && player.team.getColor() == TeamColor.Blue)
                {
                    main.GMboard.cellsGrid[player.position.x, player.position.y].SetPlayerGuid(null);
                    player.Move(Direction.Down);
                    main.GMboard.cellsGrid[player.position.x, player.position.y].SetPlayerGuid(player.guid);
                    player.board.cellsGrid[player.position.x, player.position.y].SetPlayerGuid(player.guid);
                }
                else if (player.board.cellsGrid[player.position.x, player.position.y].GetCellState() == GameMaster.Cells.CellState.Empty ||
                    player.board.cellsGrid[player.position.x, player.position.y].GetCellState() == GameMaster.Cells.CellState.Valid)
                {
                    placeButton_Click(this, null);
                    tested = false;
                }
                else if (player.board.cellsGrid[player.position.x, player.position.y].GetCellState() == GameMaster.Cells.CellState.Goal ||
                    player.board.cellsGrid[player.position.x, player.position.y].GetCellState() == GameMaster.Cells.CellState.NoGoal)
                {
                    Random rand = new Random();
                    int res = rand.Next() % 4;
                    Direction dir = Direction.Up;
                    if (res == 0)
                        dir = Direction.Down;
                    if (res == 1)
                        dir = Direction.Left;
                    if (res == 2)
                        dir = Direction.Right;
                    if (res == 3)
                        dir = Direction.Up;
                    main.GMboard.cellsGrid[player.position.x, player.position.y].SetPlayerGuid(null);
                    player.Move(dir);
                    main.GMboard.cellsGrid[player.position.x, player.position.y].SetPlayerGuid(player.guid);
                    player.board.cellsGrid[player.position.x, player.position.y].SetPlayerGuid(player.guid);
                }
            }
        }
    }
}



