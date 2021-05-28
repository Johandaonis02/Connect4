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

namespace Connect4 {
    public partial class Board : Window
    {
        DispatcherTimer gameTimer = new DispatcherTimer();
        Random rand = new Random();
        MediaPlayer player = new MediaPlayer();
        ImageBrush backgroundimage = new ImageBrush();
        List<Rectangle> itemsToClear = new List<Rectangle>();

        Player player1 = new Player();
        Player player2 = new Player();

        int[,] cells = new int[7, 6];

        int cellSize = 100;
        int boardWidth = 7; //Detta är samma sak som "width" från planeringen
        int boardHeight = 6; //Detta är samma sak som "height" från planeringen
        int boardStartX = (int)(0.5 * (1000 - 7 * 100)); // 0.5 * (width - boardWidth * cellSize);
        int boardStartY = 10;

        //dessa är default settings.
        public static bool bjornMode = false; //Sätt på för att göra björn glad.
        public static double addTime = 5;
        public static double startTime = 30;
        
        bool gameIsOn = false;
        bool reset = true;

        int turn = 0;
        int frame = 0;

        public static bool botStart = false;
        public static bool botActive = false;
        public static int maxDepth = 10;

        public void StartBoard()
        {
            Left = 0;
            Top = 0;
            InitializeComponent();

            DrawBoard();
            gameTimer.Tick += GameEngine;
            gameTimer.Interval = TimeSpan.FromMilliseconds(17);
            gameTimer.Start();

            DisplayTime();
        }

        private void GameEngine(object sender, EventArgs e)
        {
            if (gameIsOn)
            {
                if (botActive && (((turn % 2 == 1) && !botStart) || ((turn % 2 == 0) && botStart)))
                {
                    botPlacePiece();
                }
                if ((turn % 2 + 1 == 1) && !botActive)
                {
                    player1.Time -= 0.055625; //Det är inte exakt en frame per 17 millisek. Tiden ska gå ner en steg per sekund.
                    DisplayTime();

                    if (player1.Time <= 0)
                    {
                        DisplayWinner(2);
                    }
                }
                else if(!botActive)
                {
                    player2.Time -= 0.055625;
                    DisplayTime();

                    if (player2.Time <= 0)
                    {
                        DisplayWinner(1);
                    }
                }
            }
            else if (reset && frame == 0)
            {
                Ready.Content = "Ready";
                Console.WriteLine("Ready");
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

        public void DisplayWinner(int player)
        {
            WinText.Content = "Player " + player + " won";
            gameIsOn = false;
        }

        public void DisplayTime()
        {
            Time1.Content = "Player 1 time left: " + (int)player1.Time;
            Time2.Content = "Player 2 time left: " + (int)player2.Time;
        }

        public void DropPiece(int column)
        {
            for (int i = 0; i < boardHeight; i++)
            {
                if (cells[column, i] == 0)
                {
                    cells[column, i] = (turn % 2) + 1;
                    
                    int drop = (int)(3*rand.NextDouble());

                    switch (drop)
                    {
                        case 0:
                            player.Open(new Uri("../../sound/drop1.mp3", UriKind.RelativeOrAbsolute));
                            break;
                        case 1:
                            player.Open(new Uri("../../sound/drop2.mp3", UriKind.RelativeOrAbsolute));
                            break;
                        case 2:
                            player.Open(new Uri("../../sound/drop3.mp3", UriKind.RelativeOrAbsolute));
                            break;
                    }

                    player.Play();


                    if (((turn % 2 + 1) == 1) && !botActive)
                    {
                        player1.Time += addTime;
                        DisplayTime();
                    }
                    else if(!botActive)
                    {
                        player2.Time += addTime;
                        DisplayTime();
                    }

                    RemoveBoard();
                    DrawBoard();

                    if (TestIfWon((turn % 2) + 1))
                    {
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

        public void botPlacePiece()
        {
            int best;

            if (turn % 2 == 1)
            {
                best = 10000;
            }
            else
            {
                best = -10000;
            }

            int columnBotPlace = -1;

            //Console.WriteLine(turn);

            for (int columnBot = 0; columnBot < 7; columnBot++)
            {
                for (int rowBot = 0; rowBot < boardHeight; rowBot++)
                {
                    if (cells[columnBot, rowBot] == 0)
                    {
                        if (turn % 2 == 0)
                        {
                            cells[columnBot, rowBot] = 1;
                        }
                        else
                        {
                            cells[columnBot, rowBot] = 2;
                        }

                        //Console.Write(bot(turn + 1, turn + 1) + " ");
                        int c = bot(turn + 1, turn + 1, -1000000, 1000000);
                        //Console.Write(c + " ");

                        if (turn % 2 == 1)
                        {
                            if (c < best || (c == best && Math.Abs(columnBotPlace - 3) > Math.Abs(columnBot - 3)))
                            {
                                best = c;
                                columnBotPlace = columnBot;
                            }
                        }
                        else
                        {
                            if (c > best || (c == best && Math.Abs(columnBotPlace - 3) > Math.Abs(columnBot - 3)))
                            {
                                best = c;
                                columnBotPlace = columnBot;
                            }
                        }

                        cells[columnBot, rowBot] = 0;
                        break;
                    }
                }
            }

            if ((turn % 2 == 0 && botStart) || (turn % 2 == 1 && !botStart))
            {
                for (int i = 0; i < boardHeight; i++)
                {
                    if (cells[columnBotPlace, i] == 0)
                    {
                        cells[columnBotPlace, i] = (turn % 2) + 1;

                        RemoveBoard();
                        DrawBoard();

                        if (TestIfWon((turn % 2) + 1))
                        {
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

        public void displayBot()
        {
            for (int y = boardHeight - 1; y >= 0; y--)
            {
                for (int x = 0; x < boardWidth; x++)
                {
                    Console.Write(cells[x, y] + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
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
                    switch (cells[x, y])
                    {
                        case 1:
                            CellImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/pictures/bluepiece.png"));
                            break;
                        case 2:
                            CellImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/pictures/redpiece.png"));
                            break;
                        default:
                            break;
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
            player1.Time = startTime;
            player2.Time = startTime;
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
                        //1 - 7
                        if (!e.IsRepeat && 34 + 40 * i < (int)(e.Key) && (int)(e.Key) < 42 + 40 * i)
                        {
                            DropPiece((int)(e.Key) - (35 + 40 * i));
                        }
                    }
                    else
                    {
                        //0 - 6
                        if (!e.IsRepeat && (33 + 40 * i) < (int)(e.Key) && (int)(e.Key) < (41 + 40 * i))
                        {
                            DropPiece((int)(e.Key) - (34 + 40 * i));
                        }
                    }
                }
            }
        }



        //bot stuff

        public int bot(int depth, int startDepth, int alpha, int beta)
        {
            //displayBot();

            int maxRound = 42;

            if (depth >= maxDepth + startDepth && !TestIfWon(1) && !TestIfWon(2))
            {
                return (0);
            }

            int best;

            if (depth % 2 == 0)
            {
                if (TestIfWon(2))
                {
                    return (depth - maxRound - 1);
                }

                best = -1337;

                for (int column = 0; column < 7; column++)
                {
                    for (int row = 0; row < boardHeight; row++)
                    {
                        if (cells[column, row] == 0)
                        {
                            cells[column, row] = 1;

                            int a = bot(depth + 1, startDepth, alpha, beta);
                            best = Math.Max(best, a);
                            alpha = Math.Max(alpha, a);
                            
                            /*
                            int c = bot(depth + 1, startDepth);
                            if (c > best)
                            {
                                best = c;
                            }
                            */

                            cells[column, row] = 0;

                            if (beta <= alpha)
                            {
                                column = 100;
                            }
                            break;
                        }
                    }
                }
                return (best);
            }
            else
            {
                if (TestIfWon(1))
                {
                    return (maxRound + 1 - depth);
                }

                best = 1337;

                for (int column = 0; column < 7; column++)
                {
                    for (int row = 0; row < boardHeight; row++)
                    {
                        if (cells[column, row] == 0)
                        {

                            cells[column, row] = 2;
                            //display(cell);

                            int a = bot(depth + 1, startDepth, alpha, beta);
                            best = Math.Min(best, a);
                            beta = Math.Min(beta, a);
                            

                            /*
                            int c = bot(depth + 1, startDepth);
                            if (c < best)
                            {
                                best = c; //bot(depth + 1, cell)
                            }
                            */
                            cells[column, row] = 0;

                            if (beta <= alpha)
                            {
                                column = 100;
                            }
                            break;
                        }
                    }
                }
                return (best);
            }
        }
    }
}
