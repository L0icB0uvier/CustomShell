using System.Text.RegularExpressions;
using Shell.Tokens;

namespace Shell;

public static class CommandParser
{
    public static Token[] ParseCommand(string command)
    {
        const string unquotedPattern = @"
                (?<NoQuotes>
                        (?:\\.
                    |
                        [^\s""'])+
                )
            ";

        const string singleQuotesPattern = @"
                '
                    (?<SingleQuotes>
                        (?:
                                [^']
                            |
                                ''
                        )+
                    )
                '(?:\s|$)
            ";
        
        const string doubleQuotesPattern = @"
               (?<DoubleQuotes>                     #Capture group for the full pattern
                    (?:\\[\\""\&]|[^\s""])*         #Look for any non space or non escaped double quote at the begining
                        ""                          #Starting Double Quotes
                            (?:\\[\\""\&]|[^""]|"""")+   #Pattern inside double quotes
                        ""                          #Ending Double Quotes
                    (?:\\[\\""\&]|[^\s""])*         #Look for any non space or non escaped double quote at the end
                )                                   #Close the capture group
            ";
        
        string pattern = $"{unquotedPattern}|{singleQuotesPattern}|{doubleQuotesPattern}";
        var matches = Regex.Matches(command, pattern, RegexOptions.IgnorePatternWhitespace);
        var tokens = ProcessMatches(matches);
        return tokens;
    }
    
    private static Token[] ProcessMatches(MatchCollection matches)
    {
        List<Token> tokens = new List<Token>();
        
        for (int i = 0; i < matches.Count; i++)
        {
            Match current = matches[i];
            
            var tokenType = GetTokenType(current);
            
            if(tokenType == TokenType.None) continue;
            
            string tokenValue = current.Groups[(int)tokenType].Value;

            Token token;

            switch (tokenType)
            {
                case TokenType.NoQuotes:
                    token = new PlainTextToken(tokenType, tokenValue);
                    tokens.Add(token);
                    break;
                case TokenType.SingleQuotes:
                    token = new SingleQuoteToken(tokenType, tokenValue);
                    tokens.Add(token);
                    break;
                case TokenType.DoubleQuotes:
                    token = new DoubleQuoteToken(tokenType, tokenValue);
                    tokens.Add(token);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        return tokens.ToArray();
    }



    private static TokenType GetTokenType(Match match)
    {
        if (match.Groups.ContainsKey("DoubleQuotes") && match.Groups["DoubleQuotes"].Success)
            return TokenType.DoubleQuotes;
        
        if (match.Groups.ContainsKey("SingleQuotes") && match.Groups["SingleQuotes"].Success)
            return TokenType.SingleQuotes;
        
        if (match.Groups.ContainsKey("NoQuotes") && match.Groups["NoQuotes"].Success)
            return TokenType.NoQuotes;
        
        return TokenType.None;
    }
   

    private static bool IsAdjacent(MatchCollection matches, int index)
    {
        Match previous = matches[index - 1];
        int prevEnd = previous.Index + previous.Length;
        int currStart = matches[index].Index;

        bool isAdjacent = prevEnd == currStart;
        return isAdjacent;
    }
}