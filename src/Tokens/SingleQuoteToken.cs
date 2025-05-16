namespace Shell.Tokens;

public class SingleQuoteToken : Token
{
    public SingleQuoteToken(TokenType tokenType, string tokenValue) : base(tokenType, tokenValue)
    {
        TokenValue = TokenValue.Replace("''", "");
    }
}