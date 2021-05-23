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
using System.Windows.Shapes;

namespace Connect4
{
    /// <summary>
    /// Interaction logic for Manu.xaml
    /// </summary>
    public partial class Manu : Window
    {
        public Manu()
        {
            InitializeComponent();
        }

        private void button_Cick(object sender, RoutedEventArgs e)
        {
            Start.Visibility = Visibility.Hidden;
            Settings.Visibility = Visibility.Hidden;


            MainWindow BoardWindow = new MainWindow();
            BoardWindow.Board();
            
        }
    }
}
