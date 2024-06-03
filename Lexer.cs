using System;
using System.Collections.Generic;
using System.Text;

public enum TokenType
{
    LeftBrace,
    RightBrace,
    LeftBracket,
    RightBracket,
    String,
    Number,
    True,
    False,
    Null,
    Colon,
    Comma,
    EOF,
    Invalid
}

public class Token
{
    public TokenType Type { get; }
    public string Lexeme { get; }

    public Token(TokenType type, string lexeme)
    {
        Type = type;
        Lexeme = lexeme;
    }
}

public class Lexer
{
    private readonly string _input;
    private int _position;

    public Lexer(string input)
    {
        _input = input;
        _position = 0;
    }

    public List<Token> Tokenize()
    {
        var tokens = new List<Token>();

        while (_position < _input.Length)
        {
            char current = _input[_position];
            switch (current)
            {
                case '{':
                    tokens.Add(new Token(TokenType.LeftBrace, "{"));
                    _position++;
                    break;
                case '}':
                    tokens.Add(new Token(TokenType.RightBrace, "}"));
                    _position++;
                    break;
                case '[':
                    tokens.Add(new Token(TokenType.LeftBracket, "["));
                    _position++;
                    break;
                case ']':
                    tokens.Add(new Token(TokenType.RightBracket, "]"));
                    _position++;
                    break;
                case ':':
                    tokens.Add(new Token(TokenType.Colon, ":"));
                    _position++;
                    break;
                case ',':
                    tokens.Add(new Token(TokenType.Comma, ","));
                    _position++;
                    break;
                case '"':
                    tokens.Add(new Token(TokenType.String, ReadString()));
                    break;
                default:
                    if (char.IsWhiteSpace(current))
                    {
                        _position++;
                    }
                    else if (char.IsDigit(current) || current == '-')
                    {
                        tokens.Add(new Token(TokenType.Number, ReadNumber()));
                    }
                    else if (current == 't' && _input.Substring(_position, 4) == "true")
                    {
                        tokens.Add(new Token(TokenType.True, "true"));
                        _position += 4;
                    }
                    else if (current == 'f' && _input.Substring(_position, 5) == "false")
                    {
                        tokens.Add(new Token(TokenType.False, "false"));
                        _position += 5;
                    }
                    else if (current == 'n' && _input.Substring(_position, 4) == "null")
                    {
                        tokens.Add(new Token(TokenType.Null, "null"));
                        _position += 4;
                    }
                    else
                    {
                        tokens.Add(new Token(TokenType.Invalid, current.ToString()));
                        return tokens;
                    }
                    break;
            }
        }

        tokens.Add(new Token(TokenType.EOF, string.Empty));
        return tokens;
    }

    private string ReadString()
    {
        var sb = new StringBuilder();
        _position++; // Skip initial quote

        while (_position < _input.Length && _input[_position] != '"')
        {
            sb.Append(_input[_position]);
            _position++;
        }

        if (_position >= _input.Length)
        {
            throw new Exception("Unterminated string literal");
        }

        _position++; // Skip closing quote
        return sb.ToString();
    }

    private string ReadNumber()
    {
        var sb = new StringBuilder();

        while (_position < _input.Length && (char.IsDigit(_input[_position]) || _input[_position] == '.' || _input[_position] == '-'))
        {
            sb.Append(_input[_position]);
            _position++;
        }

        return sb.ToString();
    }
}
