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
        public PlayerWindow(MainWindow wind, string name, Team team)
        {
            main = wind;
            Random rand = new Random();
            player = new Player(rand.Next() % 50 , name, team, false);
            InitializeComponent();
            SetInfo(name, team);
            playerBoard = new Board(main.GMboard.boardWidth, main.GMboard.goalAreaHeight, main.GMboard.taskAreaHeight);
            player.board = playerBoard;

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

                    if (i < player.board.goalAreaHeight || i >= player.board.goalAreaHeight + player.board.taskAreaHeight)
                    {
                        txtb.BorderBrush = new SolidColorBrush(Colors.Black);
                        txtb.BorderThickness = new Thickness(1);
                    }

                    if (player.board.cellsGrid[j, i].GetCellState() == GameMaster.Cells.CellState.Goal)
                    {
                        txtb.Text = "G";
                        txtb.Background = Brushes.LightYellow;
                    }
                    else if (player.board.cellsGrid[j, i].GetCellState() == GameMaster.Cells.CellState.Piece || player.board.cellsGrid[j, i].GetCellState() == GameMaster.Cells.CellState.Sham)
                    {
                        txtb.Text = "P";
                        txtb.Background = Brushes.Black;
                        txtb.Foreground = Brushes.White;
                    }
                    else if (player.board.cellsGrid[j, i].GetPlayerGuid() == player.guid)
                    {
                        if (player.team.getColor() == TeamColor.Red)
                            txtb.Background = Brushes.Red;
                        else
                            txtb.Background = Brushes.Blue;
                        txtb.Foreground = Brushes.White;
                        txtb.Text = player.board.cellsGrid[j, i].GetPlayerGuid();
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

        private void SetInfo(string name, Team team)
        {
            nameLabel.Content = name;
            teamLabel.Content = team.color;
        }

        private void pickButton_Click(object sender, RoutedEventArgs e)
        {
            pickButton.Visibility = Visibility.Collapsed;
            placeButton.Visibility = Visibility.Visible;
            testButton.Visibility = Visibility.Visible;
        }

        private void placeButton_Click(object sender, RoutedEventArgs e)
        {
            pickButton.Visibility = Visibility.Visible;
            placeButton.Visibility = Visibility.Hidden;
            testButton.Visibility = Visibility.Collapsed;
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
        }

        //void AIMove() //simple AI for player
        //{
        //    Random rand = new Random();
        //    if (piece == null) // null or false w zależności czy piece to Piece lub bool
        //    {
        //        int dir = HamiltonianDistance.min.getposition; //get position of smallest element in list of hamiltionian distance to piece for nearby fields 
        //        //(0 - player and piece on the same field, 1 - smallest distance in the upper-left cell, 2 - smallest dist in the upper cell itp.) 
        //        switch (dir)
        //        {
        //            case 0:
        //                {
        //                    TakePiece();
        //                    break;
        //                }
        //            case 1:
        //                {
        //                    if (rand.Next() % 2 == 0)
        //                        player.Move(Direction.Left);
        //                    else
        //                        player.Move(Direction.Up);
        //                    break;
        //                }
        //            case 2:
        //                {
        //                    player.Move(Direction.Up);
        //                    break;
        //                }
        //            case 3:
        //                {
        //                    if (rand.Next() % 2 == 0)
        //                        player.Move(Direction.Right);
        //                    else
        //                        player.Move(Direction.Up);
        //                    break;
        //                }
        //            case 4:
        //                {
        //                    player.Move(Direction.Left);
        //                    break;
        //                }
        //            case 5:
        //                {
        //                    player.Move(Direction.Right);
        //                    break;
        //                }
        //            case 6:
        //                {
        //                    if (rand.Next() % 2 == 0)
        //                        player.Move(Direction.Left);
        //                    else
        //                        player.Move(Direction.Down);
        //                    break;
        //                }
        //            case 7:
        //                {
        //                    player.Move(Direction.Down);
        //                    break;
        //                }
        //            case 8:
        //                {
        //                    if (rand.Next() % 2 == 0)
        //                        player.Move(Direction.Right);
        //                    else
        //                        player.Move(Direction.Down);
        //                    break;
        //                }
        //            default: break;
        //        }
        //    }
        //    else
        //    {
        //        if(piece == undiscovered)
        //        {
        //            TestPiece();
        //        }
        //        else if (piece == sham)
        //        {
        //            DestroyPiece();
        //        }
        //        else
        //        {
        //            if (team.getColor() == TeamColor.Blue)
        //            {
        //                if (y < player.board.taskAreaHeight + player.board.goalAreaHeight)
        //                    player.Move(Direction.Up);
        //                else
        //                {
        //                    if (PiecePuttedHere)
        //                    {
        //                        player.Move(Direction.Up + rand.Next() % 3);
        //                    }
        //                    else
        //                        PutPiece();
        //                }
        //            }
        //            else
        //            {
        //                if (y > player.board.goalAreaHeight)
        //                    player.Move(Direction.Down);
        //                else
        //                {
        //                    if (PiecePuttedHere)
        //                    {
        //                        player.Move(Direction.Up + rand.Next() % 3);
        //                    }
        //                    else
        //                        PutPiece();
        //                }
        //            }
        //        }
        //    }
        //}
    }
}



