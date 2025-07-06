using Mastermind;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Help;
using System.CommandLine.Parsing;


class Program
{
    static void Main(string[] args)
    {
        string passCode= "";
        int maxAttempts;
        Option<string> passCodeOption = new("-c")
        {
            Description = "Write your own 4 digit code (don't tell anyone)."
        };
        
        Option<int> attemptOption = new("-t")
        {
            Description = "Change how many tries you get.",
            DefaultValueFactory = parseResult => 10
        };
       
        RootCommand rootCommand = new("Sample app for System.CommandLine");
        rootCommand.Options.Add(passCodeOption);
        rootCommand.Options.Add(attemptOption);
       
        ParseResult parseResult = rootCommand.Parse(args);
        //if(parseResult.GetValueForOption(HelpOption()))
        
        if (parseResult.GetValue(passCodeOption) is string parsedCode)
        {
            passCode  = parsedCode;
        }
        if (parseResult.GetValue(attemptOption) is int parsedAttempts)
        {
            maxAttempts = parsedAttempts;
        }
        foreach (ParseError parseError in parseResult.Errors)
        {
            Console.Error.WriteLine(parseError.Message);
        }
        //rootCommand.Parse("-h").Invoke();

        Gameplay gameplay = new Gameplay(passCode, maxAttempts);
        gameplay.Play();
    }
}
