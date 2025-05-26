namespace Shell.Tokens;

public class RedirectToken : Token
{
    public RedirectType RedirectType { get; private set; }
    public string RedirectPath { get; private set; }
    
    public RedirectToken(TokenType tokenType, string tokenValue) : base(tokenType, tokenValue)
    {
        var parts = tokenValue.Split(" ");
        RedirectType = ExtractRedirectType(parts[0]);
        RedirectPath = parts[1];
    }

    private RedirectType ExtractRedirectType(string redirectToken)
    {
        return redirectToken switch
        {
            ">" or "1>" => RedirectType.StandardOutput,
            "2>" => RedirectType.StandardError,
            ">>" or "1>>"  => RedirectType.AppendOutput,
            "2>>" => RedirectType.AppendError,
            _ => RedirectType.StandardOutput
        };
    }
}