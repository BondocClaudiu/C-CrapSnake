using System;
using System.Collections.Generic;
using System.Timers;

namespace Snake
{
    class Program
    {
        //If true => endgame
        static bool GameOver;
        //MxN Table dimension, Snake-Head Position,Last Direction
        static int M, N,LastDir,BoxI,BoxJ,Score;
        //Table[,]-the array for the actual operations
        static char[,] Table;
        //Tuple of i,j coordinates ,first item(M coord) , second item(N coord)
        static List<Tuple<int,int>> Snake;
        //A simple timer,used to move the snake at a given time if there are no input keys
        static Timer t;

        static void Main(string[] args)
        {
            GameOver = false;
            M = 20; N = 15;
            Table = new char[M, N];
            
            //(1,1) - Start position,last dir = 1, moving by default downwards
            Snake = new List<Tuple<int, int>>() { Tuple.Create(1,1) };
            LastDir = 1;

            //Set an Event function to draw the next move at a given time, using lastDir
            t = new Timer(100);
            t.Elapsed += T_Elapsed;

            Console.CursorVisible = false;
            StartGame();
            
        }

        private static void T_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!GameOver)
            {
                MoveAndDraw(LastDir);
            }
        }

        private static void Print()
        {
            Console.WriteLine("\t\tCrap Snake , v1.0");
            for (int i = 0; i < M; i++)
            {
                for (int j = 0; j < N; j++)
                {   
                    Console.Write(Table[i, j] + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine($"\n\n\t\tScore = {Score}");
        }

        private static void DrawTable()
        {
            for (int i = 0; i < M; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (i == 0 || i == M - 1)
                    {
                        Table[i, j] = '-';
                    }
                    else if (j == 0 || j == N - 1)
                    {
                        Table[i, j] = '|';
                    }
                    else
                    {
                        Table[i, j] = '.';
                    }
                }
            }
            foreach(var s in Snake)
            {
                Table[s.Item1, s.Item2] = 'o';
            }
            Table[Snake[0].Item1,Snake[0].Item2] = 'O';
            if (BoxI != 0 && BoxJ != 0 ) { Table[BoxI, BoxJ] = '#'; }
        }

        //Draw boxes randomly, in the positions not occupied by the Snake body
        private static void DrawBox()
        {
            Random r = new Random();
            do
            {
                BoxI = r.Next(2, M - 2);
                BoxJ = r.Next(2, N - 2);
            }
            while (Snake.Contains(Tuple.Create(BoxI,BoxJ)));

            Table[BoxI, BoxJ] = '#';
        }

        //0 = UP, 1 = DOWN, 2 = RIGHT , 3 = LEFT
        private static void Move(int direction)
        {
            int HeadI, HeadJ, TailI, TailJ;
            HeadI = Snake[0].Item1;HeadJ = Snake[0].Item2;
            TailI = Snake[Snake.Count - 1].Item1; TailJ = Snake[Snake.Count - 1].Item2;
            
            switch (direction)
            {
                case 0:
                    if(LastDir != 1 && HeadI-1 > 0 && !Snake.Contains(Tuple.Create(HeadI - 1, HeadJ)))
                    {
                        if(HeadI - 1 == BoxI && HeadJ == BoxJ)
                        {
                            Snake.Insert(0, Tuple.Create(HeadI - 1, HeadJ));
                            DrawBox();
                            Score++;
                        }
                        else {
                            Snake.Insert(0, Tuple.Create(HeadI - 1, HeadJ));
                            Snake.RemoveAt(Snake.Count - 1);
                        }
                        LastDir = 0;
                    }
                    else if (LastDir != 1 && Snake.Contains(Tuple.Create(HeadI - 1, HeadJ))) { GameOver = true; Console.WriteLine("Eaten by itself!UP"); }
                    else if(HeadI-1 == 0){ GameOver = true; }
                    break;

                case 1:
                    if (LastDir != 0 && HeadI + 1 < M-1 && !Snake.Contains(Tuple.Create(HeadI + 1, HeadJ)))
                    {
                        if (HeadI + 1 == BoxI && HeadJ == BoxJ)
                        {
                            Snake.Insert(0, Tuple.Create(HeadI + 1, HeadJ));
                            DrawBox();
                            Score++;
                        }
                        else
                        {
                            Snake.Insert(0, Tuple.Create(HeadI + 1, HeadJ));
                            Snake.RemoveAt(Snake.Count - 1);
                        }
                        LastDir = 1;
                    }
                    else if (LastDir != 0 && Snake.Contains(Tuple.Create(HeadI + 1, HeadJ))) { GameOver = true; Console.WriteLine("Eaten by itself!DOWN"); }
                    else if (HeadI + 1 == M-1) { GameOver = true; }
                    break;

                case 2:
                    if (LastDir != 3 && HeadJ + 1 < N-1 && !Snake.Contains(Tuple.Create(HeadI, HeadJ+1)))
                    {
                        if (HeadI == BoxI && HeadJ+1 == BoxJ)
                        {
                            Snake.Insert(0, Tuple.Create(HeadI, HeadJ+1));
                            DrawBox();
                            Score++;
                        }
                        else
                        {
                            Snake.Insert(0, Tuple.Create(HeadI, HeadJ+1));
                            Snake.RemoveAt(Snake.Count - 1);
                        }
                        LastDir = 2;
                    }
                    else if (LastDir != 3 && Snake.Contains(Tuple.Create(HeadI, HeadJ+1))) { GameOver = true; Console.WriteLine("Eaten by itself!RIGHT"); ; }
                    else if (HeadJ + 1 == N-1) { GameOver = true; }
                    break;

                case 3:
                    if (LastDir != 2 && HeadJ - 1 > 0 && !Snake.Contains(Tuple.Create(HeadI, HeadJ-1)))
                    {
                        if (HeadI == BoxI && HeadJ-1 == BoxJ)
                        {
                            Snake.Insert(0, Tuple.Create(HeadI, HeadJ-1));
                            DrawBox();
                            Score++;
                        }
                        else
                        {
                            Snake.Insert(0, Tuple.Create(HeadI, HeadJ-1));
                            Snake.RemoveAt(Snake.Count - 1);
                        }
                        LastDir = 3;
                    }
                    else if (LastDir != 2 && Snake.Contains(Tuple.Create(HeadI - 1, HeadJ))) { GameOver = true; Console.WriteLine("Eaten by itself!LEFT"); }
                    else if (HeadJ - 1 == 0) { GameOver = true; }
                    break;
            }
        }

        //Combine the methods above,for a easier use inside the StartGame() method
        private static void MoveAndDraw(int direction)
        {
            Move(direction);
            DrawTable();
            Console.Clear();
            Print();
        }

        private static void StartGame() {

            DrawTable();
            DrawBox();
            t.Start();

            while (!GameOver)
            {
                var input = Console.ReadKey(true);
                
                switch (input.Key)
                {
                    case ConsoleKey.UpArrow:
                        t.Stop();
                        MoveAndDraw(0);
                        t.Start();
                        break;

                    case ConsoleKey.DownArrow:
                        t.Stop();
                        MoveAndDraw(1);
                        t.Start();
                        break;

                    case ConsoleKey.RightArrow:
                        t.Stop();
                        MoveAndDraw(2);
                        t.Start();
                        break;

                    case ConsoleKey.LeftArrow:
                        t.Stop();
                        MoveAndDraw(3);
                        t.Start();
                        break;
                }
            }
            if (GameOver) {Console.WriteLine("GAME OVER!"); }
        }
    }
}
