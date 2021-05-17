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

using System.Windows.Threading;

namespace Connect4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer gameTimer = new DispatcherTimer();

        MediaPlayer player = new MediaPlayer();
        ImageBrush backgroundimage = new ImageBrush();

        int time;
        int[,] cells = {{0, 0, 0, 0, 0, 0, 0}, { 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0 }};
        public MainWindow()
        {
            InitializeComponent();
            
            gameTimer.Tick += GameEngine;
            gameTimer.Interval = TimeSpan.FromMilliseconds(17);

            /*
            backgroundimage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/pictures/cell.png"));
            Connect4.Background = backgroundimage;
            */
        }
        //test
        private void GameEngine(object sender, EventArgs e)
        {
            Time1.Content = "Score";
            //Time1.Content = "Score: " + time;
            //time++;
            //drawBoard();
            
        }

        public void removeBoard(){
            foreach (var x in Connect4.Children.OfType<Rectangle>())
            {
                Connect4.Children.Remove(x);
            }
        }

        public bool TestIfWon()
        {
            int player = 1; //Detta ska inte alltid vara 1 men jag fixar det sen.
            bool FourInARow = true;

            //row
            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 6; y++)
                {
                    FourInARow = true;
                    
                    for (int xOffset = 0; xOffset < 4; xOffset++)
                    {
                        if(cells[x + xOffset, y] != player)
                        {
                            FourInARow = false;
                        } 
                    }
                    
                    if (FourInARow)
                    {
                        return (true);
                    }
                    
                }
            }

            //column
            for (int x = 0; x < 7; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    FourInARow = true;

                    for (int yOffset = 0; yOffset < 4; yOffset++)
                    {
                        if (cells[x + yOffset, y] != player)
                        {
                            FourInARow = false;
                        }
                    }

                    if (FourInARow)
                    {
                        return (true);
                    }
                }
            }
        }
        public void drawBoard(){

            ImageBrush CellImage = new ImageBrush();
            CellImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/pictures/cell.png"));

            Rectangle newCell = new Rectangle
            {
                Tag = "cell",
                Height = 50,
                Width = 50,
                Fill = CellImage
            };

            Canvas.SetLeft(newCell, 600);
            Canvas.SetTop(newCell, 600);

            Connect4.Children.Add(newCell);
        }

        private void PlacePiece(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine("test");
        }
    }
}
