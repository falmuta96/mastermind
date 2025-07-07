using System.CommandLine.Parsing;
using System.CommandLine;

namespace Mastermind
{
    public class Gameplay
    {
        private string? Code { get; set; }
        private int Attempts { get; set; }


        public void Play()
        {

            Console.WriteLine("Can you break the code? Enter a valid guess.\n");
            int currentAttempt = 1;
            while (currentAttempt <= Attempts)
            {

                Console.Write($"Round {currentAttempt}/{Attempts}: ");
                string guess = ReadLineWithControlDetection();


                if (string.IsNullOrEmpty(guess))
                {
                    Console.WriteLine("\nThanks for playing!:)");
                    return;
                }


                if (!IsValidGuess(guess))
                {
                    Console.WriteLine("Try a different guess. Remember the code is 4 unique numbers between 0 and 8 .\n");
                    continue;
                }

                if (guess == Code)
                {
                    Console.WriteLine("Congratz! You did it!");
                    return;
                }

                (int correctPos, int correctDigit) = EvaluateGuess(guess);
                Console.WriteLine($"Well-placed pieces: {correctPos}\nMisplaced pieces: {correctDigit}\n");

                currentAttempt++;
            }
            Console.WriteLine($"Out of attempts! The passcode was: {Code}");
        }
        private static string GenerateSecretCode()
        {
            Random rand = new Random();
            string result = "";

            while (result.Length < 4)
            {
                string digit = rand.Next(0, 9).ToString();

                if (!result.Contains(digit))
                {
                    result += digit;
                }
            }

            return result;
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

        private (int, int) EvaluateGuess(string guess)
        {
            int correctPos = 0;
            int correctDigit = 0;


            for (int i = 0; i < 4; i++)
            {
                if (guess[i] == Code[i])
                {
                    correctPos++;
                }
                else if (Code.Contains(guess[i]))
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
        public bool Setup(string[] args)
        {
            Option<string> passCodeOption = new("-c")
            {
                Description = "Write your own 4 digit code (don't tell anyone)."
            };

            Option<int> attemptOption = new("-t")
            {
                Description = "Change how many tries you get.",
                DefaultValueFactory = parseResult => 10
            };

            RootCommand rootCommand = new("Mastermind");
            rootCommand.Options.Add(passCodeOption);
            rootCommand.Options.Add(attemptOption);
            ParseResult parseResult = rootCommand.Parse(args);

            if (args.Any(arg => arg is "-h" or "--help" or "-?") || parseResult.Errors.Count() > 0)
            {
                foreach (ParseError parseError in parseResult.Errors)
                {
                    Console.Error.WriteLine(parseError.Message);
                }
                rootCommand.Parse("-h").Invoke();
                return false;
            }

            if (parseResult.GetValue(passCodeOption) is string parsedCode)
            {
                Code = parsedCode;
            }
            if (parseResult.GetValue(attemptOption) is int parsedAttempts)
            {
                Attempts = parsedAttempts;
            }
            if (string.IsNullOrEmpty(Code))
            {
                Code = GenerateSecretCode();
            }
            else if (!IsValidGuess(Code))
            {
                Console.WriteLine("Hey! That passcode won't work. Remember the code has to be 4 unique numbers between 0 and 8.\n");
                return false;
            }
            return true;
        }

    }
}
