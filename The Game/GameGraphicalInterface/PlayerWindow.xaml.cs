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

namespace GameGraphicalInterface
{
    /// <summary>
    /// Interaction logic for PlayerWindow.xaml
    /// </summary>
    public partial class PlayerWindow : Window
    {
        MainWindow main;
        Player player;
        public PlayerWindow(MainWindow wind, string name, Team team)
        {
            main = wind;
            player = new Player(0, name, team, false);
            InitializeComponent();
        }

        private void upButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
