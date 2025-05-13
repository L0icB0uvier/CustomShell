using System.Text.RegularExpressions;

namespace Shell;

public static class CommandParser
{
    public static string[] ParseCommand(string command)
    {
        //@"(?<NoQuotes>[^\s""']+)|'(?<SingleQuotes>[^']*)'|(?<DoubleQuotes>""[^""]*"")"
        string pattern = @"(?<NoQuotes>(?:\\.|[^\s""'])+)|'(?<SingleQuotes>[^']+)'|""(?<DoubleQuotes>(?:\\.|[^""])+)""";
        var matches = Regex.Matches(command, pattern);
        var tokens = ProcessMatches(matches);
        return tokens;
    }
    
    private static string[] ProcessMatches(MatchCollection matches)
    {
        List<string> tokens = new List<string>();
        string currentToken = String.Empty;
        int currentGroupIndex = 0;
        
        for (int i = 0; i < matches.Count; i++)
        {
            Match current = matches[i];

            int groupIndex = current.Groups[3].Success ? 3 : current.Groups[2].Success ? 2 : 1;
            string token = current.Groups[groupIndex].Value;

            if (groupIndex == 1)
            {
                currentToken = token.Replace("\\", "");
                currentGroupIndex = groupIndex;
                tokens.Add(currentToken);
            }

            else
            {
                if (i == 0)
                {
                    currentToken = token;
                    currentGroupIndex = groupIndex;
                    continue;
                }
                
                if (groupIndex == currentGroupIndex)
                {
                    if (IsAdjacent(matches, i))
                    {
                        currentToken += token;
                        if(i == matches.Count - 1) tokens.Add(currentToken);
                    }
                    else
                    {
                        tokens.Add(currentToken);
                        currentToken = token;
                        currentGroupIndex = groupIndex;
                        if(i == matches.Count - 1) tokens.Add(currentToken);
                    }
                }

                else
                {
                    currentToken = token;
                    currentGroupIndex = groupIndex;
                    if(i == matches.Count - 1) tokens.Add(currentToken);
                }
            }
        }
        
        return tokens.ToArray();
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