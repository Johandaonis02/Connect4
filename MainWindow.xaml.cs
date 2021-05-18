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
        Random rand = new Random();
        MediaPlayer player = new MediaPlayer();
        ImageBrush backgroundimage = new ImageBrush();
        List<Rectangle> itemsToClear = new List<Rectangle>();

        

        int time;
        int[,] cells = {{0, 0, 0, 0, 0, 0}, { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 }};
        int cellSize = 100;
        int boardWidth = 7; //Detta är samma sak som "width" från planeringen
        int boardHeight = 6; //Detta är samma sak som "height" från planeringen
        int boardStartX = (int) (0.5 * (1000 - 7 * 100)); // 0.5 * (width - boardWidth * cellSize);
        int boardStartY = 10;
        int turn = 0;
        bool bjornMode = false; //Sätt på för att göra björn glad.
        double addTime = 10;
        double timePlayer1 = 10; //startTime
        double timePlayer2 = 10; //startTime
        bool gameIsOn = true;


        public MainWindow()
        {
            InitializeComponent();

            DrawBoard();
            gameTimer.Tick += GameEngine;
            gameTimer.Interval = TimeSpan.FromMilliseconds(17);
            gameTimer.Start();
            
            /*
            backgroundimage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/pictures/cell.png"));
            Connect4.Background = backgroundimage;
            */
        }
        //test
        private void GameEngine(object sender, EventArgs e)
        {
            //Time1.Content = "Score: " + time;
            //time++;

            //RemoveBoard();
            //DrawBoard();

            if (gameIsOn)
            {
                if (turn % 2 + 1 == 1)
                {
                    timePlayer1 -= 0.03125; //Det är inte exakt 60 fps. Behöver fixa tidsreduseringen
                    Time1.Content = "Player 1 time left: " + timePlayer1;

                    if (timePlayer1 <= 0)
                    {
                        DisplayWinner(2);
                    }
                }
                else
                {
                    timePlayer2 -= 0.03125;
                    Time2.Content = "Player 2 time left: " + timePlayer2;

                    if (timePlayer2 <= 0)
                    {
                        DisplayWinner(1);
                    }
                }
            }
        }

        public void DisplayWinner(int player)
        {
            WinText.Content = "Player " + player + " won";
            gameIsOn = false;
        }

        public void DropPiece(int column)
        {
            for (int i = 0; i < boardHeight; i++)
            {
                if (cells[column, i] == 0)
                {
                    cells[column, i] = (turn % 2) + 1;

                    RemoveBoard();
                    DrawBoard();

                    if (TestIfWon((turn % 2) + 1))
                    {
                        //string bingbong = "Player " + ((turn % 2) + 1) + " won";
                        //Time1.Content = "Player {0} won";
                        DisplayWinner((turn % 2) + 1);
                    }

                    turn++;
                    break;
                }
            }
            

        }
        public void RemoveBoard(){
            foreach (var x in Connect4.Children.OfType<Rectangle>()) 
            {
                itemsToClear.Add(x);
                //Connect4.Children.Remove(x);
            }
            foreach (Rectangle y in itemsToClear)
            {
                Console.WriteLine(1 + " " + itemsToClear.Count());
                Connect4.Children.Remove(y);
                Console.WriteLine(2 + " " + itemsToClear.Count());
            }
        }

        public bool TestIfWon(int player)
        {
            //Det är mycket som upprepas i denna metod så det betyder att denna metod kan optimeras.


            bool fourInARow = true;

            //row
            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 6; y++)
                {
                    fourInARow = true;
                    
                    for (int xOffset = 0; xOffset < 4; xOffset++)
                    {
                        if(cells[x + xOffset, y] != player)
                        {
                            fourInARow = false;
                            break;
                        } 
                    }
                    
                    if (fourInARow)
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
                    fourInARow = true;

                    for (int yOffset = 0; yOffset < 4; yOffset++)
                    {
                        if (cells[x, y + yOffset] != player)
                        {
                            fourInARow = false;
                            break;
                        }
                    }

                    if (fourInARow)
                    {
                        return (true);
                    }
                }
            }

            //diagonal ++
            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    fourInARow = true;

                    for (int yandxOffset = 0; yandxOffset < 4; yandxOffset++)
                    {
                        if (cells[x + yandxOffset, y + yandxOffset] != player)
                        {
                            fourInARow = false;
                            break;
                        }
                    }

                    if (fourInARow)
                    {
                        return (true);
                    }
                }
            }

            //diagonal +-
            for (int x = 0; x < 4; x++)
            {
                for (int y = 3; y < 6; y++)
                {
                    fourInARow = true;

                    for (int yandxOffset = 0; yandxOffset < 4; yandxOffset++)
                    {
                        if (cells[x + yandxOffset, y - yandxOffset] != player)
                        {
                            fourInARow = false;
                            break;
                        }
                    }

                    if (fourInARow)
                    {
                        return (true);
                    }
                }
            }

            return false;
        }
        public void DrawBoard(){

            //cells
            for (int x = 0; x < boardWidth; x++)
            {
                for (int y = 0; y < boardHeight; y++)
                {
                    ImageBrush CellImage = new ImageBrush();
                    CellImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/pictures/cell.png"));

                    Rectangle newCell = new Rectangle
                    {
                        Tag = "cell",
                        Height = cellSize,
                        Width = cellSize,
                        Fill = CellImage
                    };

                    Canvas.SetLeft(newCell, boardStartX + cellSize * x);
                    Canvas.SetBottom(newCell, boardStartY + cellSize * y);

                    Connect4.Children.Add(newCell);
                }
            }

            //piece
            for (int x = 0; x < boardWidth; x++)
            {
                for (int y = 0; y < boardHeight; y++)
                {
                    ImageBrush CellImage = new ImageBrush();
                    if (cells[x,y] == 1) // kanske ska göra en switch case
                    {
                        CellImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/pictures/bluepiece.png"));
                    }
                    else if(cells[x,y] == 2)
                    {
                        CellImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/pictures/redpiece.png"));
                    }

                    Rectangle newCell = new Rectangle
                    {
                        Tag = "cell",
                        Height = cellSize,
                        Width = cellSize,
                        Fill = CellImage
                    };

                    Canvas.SetLeft(newCell, boardStartX + cellSize * x);
                    Canvas.SetBottom(newCell, boardStartY + cellSize * y);

                    Connect4.Children.Add(newCell);
                }
            }

        }

        private void PlacePiece(object sender, MouseButtonEventArgs e)
        {
            
            
            //var point = e.GetPosition();

            //Console.WriteLine(e.GetPosition);
            
        }

        private void HandleKeyDown(object sender, KeyEventArgs e)
        {
            if (gameIsOn)
            {
                if (!bjornMode)
                {
                    if (!e.IsRepeat && 34 < (int)(e.Key) && (int)(e.Key) < 42)
                    {
                        DropPiece((int)(e.Key) - 35);
                    }
                }
                else
                {
                    if (!e.IsRepeat && 33 < (int)(e.Key) && (int)(e.Key) < 41)
                    {
                        DropPiece((int)(e.Key) - 34);
                    }
                }
            }
        }
    }
}
