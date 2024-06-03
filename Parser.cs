using System;
using System.Collections.Generic;

public class Parser
{
    private readonly List<Token> _tokens;
    private int _current;

    public Parser(List<Token> tokens)
    {
        _tokens = tokens;
        _current = 0;
    }

    public void Parse()
    {
        if (Match(TokenType.Invalid))
        {
            Console.WriteLine("Invalid JSON");
            Environment.Exit(1);
        }

        if (Match(TokenType.LeftBrace))
        {
            if (Match(TokenType.RightBrace) || ParseObject())
            {
                if (_current >= _tokens.Count || _tokens[_current].Type == TokenType.EOF)
                {
                    Console.WriteLine("Valid JSON");
                    Environment.Exit(0);
                }
            }
        }

        Console.WriteLine("Invalid JSON");
        Environment.Exit(1);
    }

    private bool ParseObject()
    {
        if (Match(TokenType.RightBrace)) return true; // Check for empty object first

        do
        {
            if (!Match(TokenType.String)) return false; // Only allow quoted keys
            if (!Match(TokenType.Colon)) return false;
            if (!ParseValue()) return false;
        } while (Match(TokenType.Comma));

        return Match(TokenType.RightBrace);
    }

    private bool ParseValue()
    {
        if (Match(TokenType.String)) return true;
        if (Match(TokenType.Number)) return true;
        if (Match(TokenType.True)) return true;
        if (Match(TokenType.False)) return true;
        if (Match(TokenType.Null)) return true;
        if (Match(TokenType.LeftBrace)) return ParseObject();
        if (Match(TokenType.LeftBracket)) return ParseArray();
        return false;
    }

    private bool ParseArray()
    {
        if (Match(TokenType.RightBracket)) return true;

        do
        {
            if (!ParseValue()) return false;
        } while (Match(TokenType.Comma));

        return Match(TokenType.RightBracket);
    }

    private bool Match(TokenType type)
    {
        if (Check(type))
        {
            _current++;
            return true;
        }

        return false;
    }

    private bool Check(TokenType type)
    {
        if (IsAtEnd()) return false;
        return _tokens[_current].Type == type;
    }

    private bool IsAtEnd()
    {
        return _current >= _tokens.Count || _tokens[_current].Type == TokenType.EOF;
    }
}
