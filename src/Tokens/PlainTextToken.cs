namespace Shell.Tokens;

public class PlainTextToken : Token
{
    public PlainTextToken(TokenType tokenType, string tokenValue) : base(tokenType, tokenValue)
    {
        TokenValue = TokenValue.Replace("\\", "");
    }
    
}