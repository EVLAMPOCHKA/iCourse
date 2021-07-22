using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace HMAC
{
    class Program
    {
        static void Main(string[] args)
        {
            if (isCorrectArgs(args))
            {
                while(true)
                {
                    string key = GenerateRandomKey(16);
                    Console.WriteLine($"HMAC key: {key}");
                    Console.WriteLine();
                    var choice = GenerateComputerChoice(args);
                    Menu(args);
                    var userMove = MakeUserMove(args.Length);

                    if (userMove != 0)
                    {
                        if (choice == userMove - 1)
                            Console.WriteLine("Both!");
                        else
                        {
                            if (IsUserWinner(choice, userMove - 1, args))
                                Console.WriteLine("You won! Congratulations!");
                            else Console.WriteLine("You lost( Try again.");
                        }
                    }
                    else Environment.Exit(0);

                    Console.WriteLine($"Your move: {args[userMove - 1]}");
                    Console.WriteLine($"PC move: {args[choice]}");
                    Console.WriteLine($"HMAC: {GenerateHMAC(args[choice], key)}");
                    Console.WriteLine("Press any key to try again");
                    Console.ReadKey();
                    Console.Clear();
                }                
            }
            else
            {
                Console.WriteLine("Sorry, not enough arguments in terminal...");
                Console.ReadKey();
            }
            
        }

        public static string GenerateRandomKey(int length)
        {
            var randomNumberGenerator = new RNGCryptoServiceProvider();
            var randomNumber = new byte[length];
            randomNumberGenerator.GetBytes(randomNumber);
            return BitConverter.ToString(randomNumber).Replace("-", "");
        }

        public static int GenerateComputerChoice(string[] moves)
        {
            Random random = new Random();
            return random.Next(0, moves.Length);
        }

        public static string GenerateHMAC(string move, string key)
        {
            byte[] byteKey = Encoding.Default.GetBytes(key);
            var hmac = new HMACSHA256(byteKey);
            byte[] byteChoice = Encoding.Default.GetBytes(move);
            var hmacHash = hmac.ComputeHash(byteChoice);
            return BitConverter.ToString(hmacHash).Replace("-", "");
        }

        public static bool IsUserWinner(int computerMove, int userMove, string[] moves)
        {
            return !(((computerMove > userMove) && (computerMove <= userMove + moves.Length / 2)) ||
                ((computerMove >= 0) && (computerMove < userMove - moves.Length / 2)));
        }

        public static void Menu(string[] moves)
        {
            Console.WriteLine("Hi! Choose your move:");
            int i = 1;
            foreach (string move in moves)
                Console.WriteLine($"#{i++} {move}.");
            Console.WriteLine("#0 Exit.");
        }

        public static bool CheckInput(string move, int length)
        {
            int userMove;
            if (int.TryParse(move, out userMove))
            {
                int[] a = new int[length + 1];
                for (int i = 0; i < length + 1; i++)
                    a[i] = i;
                foreach (int b in a)
                    if (b == userMove) return true;
            }
            Console.WriteLine("Mistake! Please, enter the correct number.");
            return false;
        }

        public static int MakeUserMove(int length)
        {
            string userMove;
            do
            {
                userMove = Console.ReadLine();
            }
            while (!CheckInput(userMove, length));
            return int.Parse(userMove);
        }

        public static bool isCorrectArgs(string[] moves)
        {
            if (moves.Length != 0 && moves.Length != 1 && moves.Length % 2 != 0)
            {
                foreach (string move in moves)
                {
                    string[] number = Array.FindAll(moves, x => x == move);
                    if (number.Length > 1) return false;
                }
                return true;
            }
            else
                return false;
            
        }

    }
}
