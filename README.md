# Mastermind

A C# console implementation of the classic game Mastermind.

## Overview

This project is a console-based version of the code-breaking game **Mastermind**. The computer randomly generates a 4-digit secret code where each digit is unique and between 0 and 8. The player must guess the code within a limited number of attempts. After each guess, the game provides feedback on:

- **Well-placed pieces:** Digits that are correct and in the correct position.
- **Misplaced pieces:** Digits that are correct but in the wrong position.

You win if you guess the code within the allowed attempts. The game also supports custom codes and attempt limits.

## Features

- Randomly generates a 4-digit code with unique numbers from 0 to 8.
- Optionally, lets you set your own secret code.
- Configurable number of attempts.
- Input validation for guesses (must be 4 unique digits between 0 and 8).
- Command-line interface with options for customization.

## How to Play

1. **Run the program:**
   ```
   dotnet run
   ```
   or, if compiled:
   ```
   ./mastermind
   ```

2. **Objective:**  
   Guess the 4-digit secret code in the allowed number of tries.

3. **Input:**  
   Enter a guess (4 unique digits between 0 and 8) when prompted.

4. **Feedback:**  
   - The game will tell you how many numbers are correct and in the right place ("well-placed") and how many are correct but in the wrong place ("misplaced").
   - If you guess the code, you win! If you run out of attempts, the secret code is revealed.

5. **Exit:**  
   Press `Ctrl+D` (or `Ctrl+Z` on Windows) to quit early.

## Command-Line Options

- `-c <code>` : Set your own 4-digit secret code (for another player to guess).
- `-t <number>` : Set the number of allowed attempts (default is 10).
- `-h`, `--help`, or `-?` : Show help and usage information.

Example:
```
dotnet run -- -c 1234 -t 8
```

## Example Session

```
Can you break the code? Enter a valid guess.

Round 1/10: 1234
Well-placed pieces: 1
Misplaced pieces: 2

Round 2/10: 5678
Well-placed pieces: 0
Misplaced pieces: 1

...
```

## Requirements

- [.NET SDK](https://dotnet.microsoft.com/) (for building/running)

## How it Works

- The secret code is generated randomly (or can be set via `-c`).
- Each guess is checked for valid format and uniqueness.
- The game evaluates and prints feedback for each guess until the player wins or runs out of attempts.

## License

This project currently does not specify a license.

---
**Author:** [falmuta96](https://github.com/falmuta96)
