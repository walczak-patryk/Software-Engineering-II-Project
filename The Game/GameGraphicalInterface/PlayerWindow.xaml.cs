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
        public PlayerWindow(MainWindow wind, string name, Team team, int boardWidth, int goalHeight, int taskHeight)
        {
            main = wind;
            player = new Player(0, name, team, false);
            InitializeComponent();
            SetInfo(name, team);
            playerBoard = new Board(boardWidth, goalHeight, taskHeight);
            DrawPlayerBoard();
        }

        private void DrawPlayerBoard()
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

                    if (i < playerBoard.goalAreaHeight || i >= playerBoard.goalAreaHeight + playerBoard.taskAreaHeight)
                    {
                        txtb.BorderBrush = new SolidColorBrush(Colors.Black);
                        txtb.BorderThickness = new Thickness(1);
                    }

                    if (playerBoard.cellsGrid[j, i].GetCellState() == GameMaster.Cells.CellState.Goal)
                    {
                        txtb.Text = "G";
                        txtb.Background = Brushes.LightYellow;
                    }
                    if (playerBoard.cellsGrid[j, i].GetCellState() == GameMaster.Cells.CellState.Piece || playerBoard.cellsGrid[j, i].GetCellState() == GameMaster.Cells.CellState.Sham)
                    {
                        txtb.Text = "P";
                        txtb.Background = Brushes.Black;
                        txtb.Foreground = Brushes.White;
                    }

                    stkp.Children.Add(txtb);
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
    }
}
