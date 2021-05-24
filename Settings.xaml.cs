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
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public Settings()
        {
            InitializeComponent();   
        }

        private void ReturnToManu(object sender, RoutedEventArgs e)
        {
            try {
                Board.startTime = Int16.Parse(StartTimeText.Text);
                Board.addTime = Int16.Parse(AddTimeText.Text);


                Board BoardWindow = new Board();
                BoardWindow.Show();
                BoardWindow.StartBoard();
                this.Close();

                /*
                MainWindow MainWindow = new MainWindow();
                MainWindow.Show();
                this.Close();
                */
            }
            catch (Exception)
            {
                Console.WriteLine("Error");
                throw;
            }
           
        }
    }
}
