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

namespace Connect4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private void GoToGame(object sender, RoutedEventArgs e)
        {
            //Left = 100;
            Board BoardWindow = new Board();
            BoardWindow.Show();
            BoardWindow.StartBoard();
            this.Close();
        }

        private void GoToSettings(object sender, RoutedEventArgs e)
        {
            Settings SettingsWindow = new Settings();
            SettingsWindow.Show();
            this.Close();
        }
    }
}
