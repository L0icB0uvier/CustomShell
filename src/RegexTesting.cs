using System.Text;
using System.Text.RegularExpressions;

namespace Shell;

public class RegexTesting
{
    public void Start()
    {
        //Test();
        TestReadInput();
    }

    private static void TestReadInput()
    {
        string doubleQuotesPattern = @"
                           (?<FullPattern>                      #Capture group for the full pattern
                                (?:\\[\\""\&]|[^\s""])*         #Look for any non space or non escaped double quote at the begining
                                    ""                          #Starting Double Quotes
                                        (?:\\[\\""\&]|[^""])+   #Pattern inside double quotes
                                    ""                          #Ending Double Quotes
                                (?:\\[\\""\&]|[^\s""])*         #Look for any non space or non escaped double quote at the end
                            )                                   #Close the capture group
                        ";
        //string pattern = @"(?<NoQuotes>(?:\\.|[^\s""'])+)|'(?<SingleQuotes>(?:[^']|[''])+)'|""(?<DoubleQuotes>(?:(?<Escaped>\\[\\""\&])|[^""])+)""";

        while (true)
        {
            Console.Write("&");
            string? input = Console.ReadLine();
            
            if (string.IsNullOrEmpty(input)) continue;
            
            var matches = Regex.Matches(input, doubleQuotesPattern, RegexOptions.IgnorePatternWhitespace);

            if (matches.Count == 0)
            {
                Console.WriteLine($"No matches found for {input}");
                continue;
            }
            
            
            Console.WriteLine($"\nNumber of Matches: {matches.Count}");
            Console.WriteLine($"Number of groups: {matches[0].Groups.Count}\n");
            
            foreach (Match match in matches)
            {
                StringBuilder sb = new StringBuilder();
                int offset = 0;
                Console.WriteLine($"Value: {match.Value}");
                Console.WriteLine("Groups:");
                foreach (Group matchGroup in match.Groups)
                {
                    
                    if(matchGroup.Success == false) continue;
                    Console.WriteLine($"{matchGroup.Name}: {matchGroup.Success} - Value: {matchGroup.Value}");
                    if(matchGroup.Captures.Count > 0)
                    {
                        Console.WriteLine("Captures:");
                        foreach (Capture capture in matchGroup.Captures)
                        {
                            Console.WriteLine($"  {capture.Value}");
                        }
                        Console.WriteLine();

                        if (matchGroup.Name == "DoubleQuotes")
                        {
                            sb.Append(matchGroup.Value);
                            offset = matchGroup.Index;
                        }

                        if (matchGroup.Name == "Escaped")
                        {
                            for (int i = matchGroup.Captures.Count - 1; i >= 0; i--)
                            {
                                sb.Remove(matchGroup.Captures[i].Index - offset, 1);
                            }
                        }
                        Console.WriteLine($"Final value: {sb.ToString()}");
                    }
                }
                Console.WriteLine();
            }
        }
    }

    private void Test()
    {
        string input = "\\hello\\";
        var unescapedInput = Regex.Escape(input);
        string pattern = @"\\[\\""n](\w+)\\";
        Match match = Regex.Match(unescapedInput, pattern);

        if (match.Success)
        {
            Console.WriteLine("Captured word: " + match.Value);  // Output: hello
        }
        else
        {
            Console.WriteLine("No match");
        }
    }
}