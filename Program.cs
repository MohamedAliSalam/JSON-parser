using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Please provide a JSON file.");
            return;
        }

        string filePath = args[0];
        string json = File.ReadAllText(filePath);

        Lexer lexer = new Lexer(json);
        var tokens = lexer.Tokenize();

        Parser parser = new Parser(tokens);
        parser.Parse();
    }
}
