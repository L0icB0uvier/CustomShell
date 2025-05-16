using System.Text.RegularExpressions;

namespace Shell.Tokens;

public class DoubleQuoteToken : Token
{
    public DoubleQuoteToken(TokenType tokenType, string tokenValue) : base(tokenType, tokenValue)
    {
        TokenValue = Regex.Replace(TokenValue, @"(?<!\\)""|(?<=\\\\)""", "");
        TokenValue = Regex.Replace(TokenValue, @"\\""|\\\\|\\\&]", match => $"{match.Value[1]}");
        TokenValue = TokenValue.Replace("\"\"", "");
    }
}