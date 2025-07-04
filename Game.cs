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
            Console.WriteLine("Can you break the code? Enter a valid guess.\n");

            //string passCode = GenerateSecretCode();
            int attempts = maxAttempts;

            if (string.IsNullOrEmpty(passCode))
            {
                passCode = GenerateSecretCode();
            }
            else if (!IsValidGuess(passCode))
            {
                Console.WriteLine("Invalid Passcode \n");
                return;
            }

            while (attempts > 0)
            {
                Console.Write($"Attempt {maxAttempts - attempts+1}/{maxAttempts}: " );
                string guess = Console.ReadLine();

                if (!IsValidGuess(guess))
                {
                    Console.WriteLine("Invalid guess. Enter 4 unique digits between 0 and 8.\n");
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
            Console.WriteLine($"Out of attempts! The pass code was: {passCode}");
        }
        private static string GenerateSecretCode()
        {
            Random rand = new Random();
            List<int> digits = new List<int>();

            while (digits.Count < 4)
            {
                int num = rand.Next(0,9); // digits from 0 to 8
                if (!digits.Contains(num))
                    digits.Add(num);
            }

            return string.Join("", digits);
        }
        static bool IsValidGuess(string guess)
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
    }
}
