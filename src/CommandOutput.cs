namespace Shell;

public class CommandOutput(string outputText, bool isError = false)
{
    public readonly string OutputText = outputText;
    public readonly bool IsError = isError;
}