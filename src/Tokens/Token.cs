namespace Shell;

public abstract class Token()
{
    public Token(TokenType tokenType, string tokenValue) : this()
    {
        TokenType = tokenType;
        TokenValue = tokenValue;
    }
    
    public TokenType TokenType;
    public string TokenValue;
}