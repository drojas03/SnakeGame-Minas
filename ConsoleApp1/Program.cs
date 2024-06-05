using System;
using System.Collections.Generic;
using System.Threading;

namespace ConsoleApp1
{
    class Program
    {
        static int width = 40;
        static int height = 20;
        static int score = 0;
        static int delay = 100;
        static bool gameOver = false;
        static Random random = new Random();

        static List<int[]> snake = new List<int[]>();
        static int[] food = new int[2];
        static int[] mine = new int[2];

        static int dx = 1; // Dirección X inicial
        static int dy = 0; // Dirección Y inicial

        static void Main(string[] args)
        {
            while (true) // Bucle externo para permitir reiniciar el juego
            {
                StartGame(); // Función para iniciar el juego

                Console.SetCursorPosition(width / 2 - 5, height / 2);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Game Over TT^TT");
                Console.SetCursorPosition(width / 2 - 8, height / 2 + 1);
                Console.Write($"Score: {score}");

                Console.SetCursorPosition(width / 2 - 10, height / 2 + 3);
                Console.Write("Press any key to play again");

                Console.ReadKey(); // Esperar a que el jugador presione una tecla para reiniciar
                Console.Clear(); // Limpiar la consola para reiniciar el juego
            }
        }

        static void StartGame()
        {
            Console.Title = "Snake Game";
            Console.CursorVisible = false;
            Console.SetWindowSize(width + 1, height + 2);
            Console.SetBufferSize(width + 1, height + 2);

            score = 0;
            gameOver = false;
            snake.Clear();
            dx = 1;
            dy = 0;

            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.Clear();

            DrawBorder();
            DrawFood();
            DrawMine();
            DrawSnake();

            Thread inputThread = new Thread(ReadInput);
            inputThread.Start();

            while (!gameOver)
            {
                MoveSnake();
                if (IsEatingFood())
                {
                    score++;
                    DrawFood();
                    DrawMine(); // Generar una nueva mina cada vez que se come la comida
                }
                if (IsTouchingMine())
                {
                    gameOver = true;
                }
                Thread.Sleep(delay);
            }
        }

        static void DrawBorder()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Blue;

            // Draw horizontal borders
            for (int i = 0; i < width + 2; i++)
            {
                if (i <= width) // Asegurarse de no exceder el ancho del campo de juego
                {
                    Console.SetCursorPosition(i, 0);
                    Console.Write("=");
                    Console.SetCursorPosition(i, height + 1);
                    Console.Write("=");
                }
            }

            // Draw vertical borders
            for (int i = 1; i < height + 1; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("|");
                Console.SetCursorPosition(height + 1, i);
                Console.Write("|");
            }
        }

        static void DrawFood()
        {
            food[0] = random.Next(1, width);
            food[1] = random.Next(1, height);

            Console.SetCursorPosition(food[0], food[1]);
            Console.ForegroundColor = ConsoleColor.Yellow; // Cambio de color a amarillo
            Console.Write("@");
        }

        static void DrawMine()
        {
            mine[0] = random.Next(1, width);
            mine[1] = random.Next(1, height);

            Console.SetCursorPosition(mine[0], mine[1]);
            Console.ForegroundColor = ConsoleColor.Green; // Cambio de color a verde
            Console.Write("°");
        }

        static void DrawSnake()
        {
            snake.Clear();
            snake.Add(new int[] { width / 2, height / 2 });
            Console.SetCursorPosition(snake[0][0], snake[0][1]);
            Console.ForegroundColor = ConsoleColor.Red; // Cambio de color a rojo
            Console.Write("#");
        }

        static void MoveSnake()
        {
            int[] newHead = { snake[0][0] + dx, snake[0][1] + dy };

            // Verificar si la nueva cabeza colisiona con el cuerpo de la serpiente
            for (int i = 1; i < snake.Count; i++)
            {
                if (newHead[0] == snake[i][0] && newHead[1] == snake[i][1])
                {
                    gameOver = true;
                    return;
                }
            }

            if (newHead[0] <= 0 || newHead[0] >= width + 1 || newHead[1] <= 0 || newHead[1] >= height + 1)
            {
                gameOver = true;
                return;
            }

            Console.SetCursorPosition(snake[snake.Count - 1][0], snake[snake.Count - 1][1]);
            Console.Write(" ");
            snake.RemoveAt(snake.Count - 1);

            snake.Insert(0, newHead);
            Console.SetCursorPosition(newHead[0], newHead[1]);
            Console.ForegroundColor = ConsoleColor.Red; // Cambio de color a rojo
            Console.Write("#");
        }

        static bool IsEatingFood()
        {
            if (snake[0][0] == food[0] && snake[0][1] == food[1])
            {
                snake.Add(new int[] { food[0], food[1] });
                return true;
            }
            return false;
        }

        static bool IsTouchingMine()
        {
            if (snake[0][0] == mine[0] && snake[0][1] == mine[1])
            {
                return true;
            }
            return false;
        }

        static void ReadInput()
        {
            while (!gameOver)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (dy != 1) { dx = 0; dy = -1; }
                        break;
                    case ConsoleKey.DownArrow:
                        if (dy != -1) { dx = 0; dy = 1; }
                        break;
                    case ConsoleKey.LeftArrow:
                        if (dx != 1) { dx = -1; dy = 0; }
                        break;
                    case ConsoleKey.RightArrow:
                        if (dx != -1) { dx = 1; dy = 0; }
                        break;
                    default:
                        gameOver = true;
                        break;
                }
            }
        }
    }
}
