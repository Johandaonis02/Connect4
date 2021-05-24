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
using System.Threading;

using System.Windows.Threading;

namespace Connect4
{
    /// <summary>
    /// Interaction logic for Board.xaml
    /// </summary>
    public partial class Board : Window
    {
        DispatcherTimer gameTimer = new DispatcherTimer();
        Random rand = new Random();
        MediaPlayer player = new MediaPlayer();
        ImageBrush backgroundimage = new ImageBrush();
        List<Rectangle> itemsToClear = new List<Rectangle>();

        //int[,] cells = {{0, 0, 0, 0, 0, 0}, { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 }};

        int[,] cells = new int[7, 6];

        int cellSize = 100;
        int boardWidth = 7; //Detta är samma sak som "width" från planeringen
        int boardHeight = 6; //Detta är samma sak som "height" från planeringen
        int boardStartX = (int)(0.5 * (1000 - 7 * 100)); // 0.5 * (width - boardWidth * cellSize);
        int boardStartY = 10;
        bool bjornMode = false; //Sätt på för att göra björn glad.
        double addTime = 1;
        static double startTime = 30;
        double timePlayer1 = startTime; //startTime
        double timePlayer2 = startTime; //startTime
        bool gameIsOn = false;
        bool reset = true;

        int turn = 0;
        int frame = 0;

        public void StartBoard()
        {
            Console.WriteLine("test");
            InitializeComponent();

            DrawBoard();
            gameTimer.Tick += GameEngine;
            gameTimer.Interval = TimeSpan.FromMilliseconds(17);
            gameTimer.Start();

            DisplayTime();
        }

        /*
        public MainWindow()
        {
            InitializeComponent();

            DrawBoard();
            gameTimer.Tick += GameEngine;
            gameTimer.Interval = TimeSpan.FromMilliseconds(17);
            gameTimer.Start();
            
            DisplayTime();

            


            //Time1.Content = "Player 1 time left: " + timePlayer1;
            //Time2.Content = "Player 2 time left: " + timePlayer2;


            
            backgroundimage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/pictures/cell.png"));
            Connect4.Background = backgroundimage;
            
        }
        */
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
                    timePlayer1 -= 0.055625; //Det är inte exakt 60 fps. Behöver fixa tidsreduseringen
                    DisplayTime();
                    //Time1.Content = "Player 1 time left: " + timePlayer1;

                    if (timePlayer1 <= 0)
                    {
                        DisplayWinner(2);
                    }
                }
                else
                {
                    timePlayer2 -= 0.055625;
                    DisplayTime();
                    //Time2.Content = "Player 2 time left: " + timePlayer2;

                    if (timePlayer2 <= 0)
                    {
                        DisplayWinner(1);
                    }
                }
            }
            else if (reset && frame == 0)
            {
                Ready.Content = "Ready";
                Console.WriteLine("Ready");
                /*
                Ready.Content = "Ready";
                Console.WriteLine("Ready");
                //Thread.Sleep(1000);
                Ready.Content = "Set";
                Console.WriteLine("Set");
                //Thread.Sleep(1000);
                Ready.Content = "Go";
                Console.WriteLine("Go");
                //Thread.Sleep(1000
                gameIsOn = true;
                reset = false;
                */
            }
            else if (reset && frame == 20)
            {
                Ready.Content = "Set";
                Console.WriteLine("Set");
            }
            else if (reset && frame == 40)
            {
                Ready.Content = "Go";
                Console.WriteLine("Go");
                gameIsOn = true;
                reset = false;
            }

            if (frame == 60)
            {
                Ready.Content = "";
            }
            frame++;
        }

        public int bot(int depth, int[,] board)
        {

            return 1;
        }
        public void DisplayWinner(int player)
        {
            WinText.Content = "Player " + player + " won";
            gameIsOn = false;
        }

        public void DisplayTime()
        {
            Time1.Content = "Player 1 time left: " + (int)timePlayer1;
            Time2.Content = "Player 2 time left: " + (int)timePlayer2;
        }

        public void DropPiece(int column)
        {
            for (int i = 0; i < boardHeight; i++)
            {
                if (cells[column, i] == 0)
                {
                    cells[column, i] = (turn % 2) + 1;

                    if ((turn % 2 + 1) == 1)
                    {
                        timePlayer1 += addTime;
                        DisplayTime();
                    }
                    else
                    {
                        timePlayer2 += addTime;
                        DisplayTime();
                    }

                    RemoveBoard();
                    DrawBoard();

                    if (TestIfWon((turn % 2) + 1))
                    {
                        //string bingbong = "Player " + ((turn % 2) + 1) + " won";
                        //Time1.Content = "Player {0} won";
                        DisplayWinner((turn % 2) + 1);
                    }
                    if ((turn + 1) >= 42)
                    {
                        WinText.Content = "Draw";
                        gameIsOn = false;
                    }

                    turn++;
                    break;
                }
            }


        }

        //Denna metod tar barnen och först lägger dem i en kista och sedan dödar dem en i taget. 
        public void RemoveBoard()
        {
            foreach (var x in Connect4.Children.OfType<Rectangle>()) //Man kan inte ändra på Connect4.Children när man looper igenom alla så man måste först lägga dem i en lista och sedan ta bart dem.
            {
                itemsToClear.Add(x);
            }
            foreach (Rectangle y in itemsToClear)
            {
                Connect4.Children.Remove(y);
            }
        }

        public bool TestIfWon(int player)
        {
            bool fourInARow = true;

            //row
            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 6; y++)
                {
                    fourInARow = true;

                    for (int xOffset = 0; xOffset < 4; xOffset++)
                    {
                        if (cells[x + xOffset, y] != player)
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

        public void DrawBoard()
        {

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
                    if (cells[x, y] == 1) // kanske ska göra en switch case
                    {
                        CellImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/pictures/bluepiece.png"));
                    }
                    else if (cells[x, y] == 2)
                    {
                        CellImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/pictures/redpiece.png"));
                    }

                    Rectangle newCell = new Rectangle
                    {
                        Tag = "piece",
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
            //Ska lägga till så man kan klicka ut bitar istället för att klicka på siffor.

            //var point = e.GetPosition();

            //Console.WriteLine(e.GetPosition);

        }

        public void ResetGame()
        {
            for (int x = 0; x < boardWidth; x++)
            {
                for (int y = 0; y < boardHeight; y++)
                {
                    cells[x, y] = 0;
                }
            }

            RemoveBoard();
            DrawBoard();

            turn = 0;
            timePlayer1 = startTime;
            timePlayer2 = startTime;
            WinText.Content = "";
            DisplayTime();

            frame = 0;
            reset = true;
            gameIsOn = false;
        }
        private void HandleKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.R)
            {
                ResetGame();
            }

            if (gameIsOn)
            {
                for (int i = 0; i <= 1; i++) //Denna loop finns för att både numpaden ska funka och för att de "vanliga" sifforna ska funka.
                {
                    if (!bjornMode)
                    {
                        if (!e.IsRepeat && 34 + 40 * i < (int)(e.Key) && (int)(e.Key) < 42 + 40 * i)
                        {
                            DropPiece((int)(e.Key) - (35 + 40 * i));
                        }
                    }
                    else
                    {
                        if (!e.IsRepeat && (33 + 40 * i) < (int)(e.Key) && (int)(e.Key) < (41 + 40 * i))
                        {
                            DropPiece((int)(e.Key) - (34 + 40 * i));
                        }
                    }
                }
            }
        }
    }
}
