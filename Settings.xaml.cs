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
            Left = 0;
            Top = 0;
        }

        private void ReturnToManu(object sender, RoutedEventArgs e)
        {
            bool error = false;
            try {
                Board.startTime = Int16.Parse(StartTimeText.Text);
                Board.addTime = Int16.Parse(AddTimeText.Text);

                string BjörnModeText = BjörnText.Text;
                switch (BjörnModeText.ToLower())
                {
                    case "true":
                    case "yes":
                    case "ja":
                        Board.bjornMode = true;
                        break;
                    case "false":
                    case "no":
                    case "nej":
                    case "nein":
                    case "fuck björn":
                        break;
                    default: Console.WriteLine("Error");
                        error = true;
                        break;
                }

                if (!error)
                {
                    Board BoardWindow = new Board();
                    BoardWindow.Show();
                    BoardWindow.StartBoard();
                    this.Close();
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Error");
                //throw;
            }
           
        }
    }
}
