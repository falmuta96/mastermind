using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Mastermind
{
    public class Gameplay([Optional] string passCode, [Optional] int maxAttempts)
    {
        public void Play()
        {          
            //string passCode = GenerateSecretCode();
            int attempts = maxAttempts;

            if (string.IsNullOrEmpty(passCode))
            {
                passCode = GenerateSecretCode();
            }
            else if (!IsValidGuess(passCode))
            {
                Console.WriteLine("Hey! That passcode won't work. Remember the code has to be 4 unique numbers between 0 and 8.\n");
                return;
            }
            
            Console.WriteLine("Can you break the code? Enter a valid guess.\n");

            while (attempts > 0)
            {
                Console.Write($"Round {maxAttempts - attempts+1}/{maxAttempts}: " );
                string guess = ReadLineWithControlDetection();


                if (string.IsNullOrEmpty(guess))
                {
                    //Console.WriteLine("\nEOF");
                    return;
                }


                if (!IsValidGuess(guess))
                {
                    Console.WriteLine("Try a different guess. Remember the code is 4 unique numbers between 0 and 8 .\n");
                    continue;
                }

                if (guess == passCode)
                {
                    Console.WriteLine("Congratz! You did it!");
                    return;
                }

                (int correctPos, int correctDigit) = EvaluateGuess(passCode, guess);
                Console.WriteLine($"Well-placed pieces: {correctPos}\nMisplaced pieces: {correctDigit}\n");

                attempts--;
            }
            Console.WriteLine($"Out of attempts! The passcode was: {passCode}");
        }
        private static string GenerateSecretCode()
        {
            Random rand = new Random();
            int[] digits = new int[4];

            for (int i=0; i<4; i++)
            {
                int num = rand.Next(0,9); // digits from 0 to 8
                if (!digits.Contains(num))
                    digits[i]=num;
            }

            return string.Join("", digits);
        }
        
        private static bool IsValidGuess(string guess)
        {
            if (guess.Length != 4) return false;

            HashSet<char> digits = new HashSet<char>();

            foreach (char c in guess)
            {
                if (!char.IsDigit(c) || c > '8') return false;
                if (!digits.Add(c)) return false; // duplicate digit
            }

            return true;
        }

        private static (int, int) EvaluateGuess(string code, string guess)
        {
            int correctPos = 0;
            int correctDigit = 0;

            for (int i = 0; i < 4; i++)
            {
                if (guess[i] == code[i])
                {
                    correctPos++;
                }
                else if (code.Contains(guess[i]))
                {
                    correctDigit++;
                }
            }

            return (correctPos, correctDigit);
        }
        private static string ReadLineWithControlDetection()
        {
            string input = "";
            ConsoleKeyInfo key;

            while (true)
            {
                key = Console.ReadKey(intercept: true);

                // Detect Ctrl+D
                if (key.Key == ConsoleKey.D && key.Modifiers.HasFlag(ConsoleModifiers.Control) || 
                    key.Key == ConsoleKey.Z && key.Modifiers.HasFlag(ConsoleModifiers.Control))
                {
                    //Console.WriteLine("\n[Ctrl+D Detected]");
                    return null; // Simulate EOF
                }

                // Detect Enter key — finish line
                if (key.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine(); // Move to next line
                    break;
                }

                // Handle Backspace
                if (key.Key == ConsoleKey.Backspace)
                {
                    if (input.Length > 0)
                    {
                        input = input.Substring(0, input.Length - 1);
                        Console.Write("\b \b");
                    }
                    continue;
                }

                // Ignore other control characters
                if (char.IsControl(key.KeyChar))
                    continue;

                // Add character to input
                input += key.KeyChar;
                Console.Write(key.KeyChar);
            }

            return input;
        }
    }
}
