public static class ConsoleExtension
{
    public static void WriteInfo(string message)
    {
        SetColor(ConsoleColor.White);
        WriteMessageAndResetColor(message);
    }
    
    public static void WriteSuccess(string message)
    {
        SetColor(ConsoleColor.Green);
        WriteMessageAndResetColor(message);
    }
    
    public static void WriteWarning(string message)
    {
        SetColor(ConsoleColor.Yellow);
        WriteMessageAndResetColor(message);
    }
    
    public static void WriteError(string message)
    {
        SetColor(ConsoleColor.Red);
        WriteMessageAndResetColor(message);
    }

    private static void SetColor(ConsoleColor color)
        => Console.ForegroundColor = color;

    private static void WriteMessageAndResetColor(string message)
    {
        Console.WriteLine(message);
        Console.ResetColor();
    }
}

//class Program
//{
//    static async Task<int> Main(string[] args)
//    {
//        while (true)
//        {
//            var args2 = Console.ReadLine().Split(null);

//            var fileOption = new Option<FileInfo?>(
//                name: "--file",
//                description: "An option whose argument is parsed as a FileInfo",
//                isDefault: true,
//                parseArgument: result =>
//                {
//                    if (result.Tokens.Count == 0)
//                    {
//                        return new FileInfo("sampleQuotes.txt");

//                    }
//                    string? filePath = result.Tokens.Single().Value;
//                    if (!File.Exists(filePath))
//                    {
//                        result.ErrorMessage = "File does not exist";
//                        return null;
//                    }
//                    else
//                    {
//                        return new FileInfo(filePath);
//                    }
//                });

//            var delayOption = new Option<int>(
//                name: "--delay",
//                description: "Delay between lines, specified as milliseconds per character in a line.",
//                getDefaultValue: () => 42);

//            var fgcolorOption = new Option<ConsoleColor>(
//                name: "--fgcolor",
//                description: "Foreground color of text displayed on the console.",
//                getDefaultValue: () => ConsoleColor.White);

//            var lightModeOption = new Option<bool>(
//                name: "--light-mode",
//                description: "Background color of text displayed on the console: default is black, light mode is white.");

//            var searchTermsOption = new Option<string[]>(
//                 name: "--search-terms",
//                 description: "Strings to search for when deleting entries.")
//            {
//                IsRequired = true,
//                AllowMultipleArgumentsPerToken = true
//            };

//            var quoteArgument = new Argument<string>(
//                name: "quote",
//                description: "Text of quote.");

//            var bylineArgument = new Argument<string>(
//                name: "byline",
//                description: "Byline of quote.");

//            var rootCommand = new RootCommand("Sample app for System.CommandLine");
//            rootCommand.AddGlobalOption(fileOption);

//            var quotesCommand = new Command("quotes", "Work with a file that contains quotes.");
//            rootCommand.AddCommand(quotesCommand);

//            var readCommand = new Command("read", "Read and display the file.")
//        {
//            delayOption,
//            fgcolorOption,
//            lightModeOption
//        };
//            quotesCommand.AddCommand(readCommand);

//            var deleteCommand = new Command("delete", "Delete lines from the file.");
//            deleteCommand.AddOption(searchTermsOption);
//            quotesCommand.AddCommand(deleteCommand);

//            var addCommand = new Command("add", "Add an entry to the file.");
//            addCommand.AddArgument(quoteArgument);
//            addCommand.AddArgument(bylineArgument);
//            addCommand.AddAlias("insert");
//            quotesCommand.AddCommand(addCommand);

//            readCommand.SetHandler(async (file, delay, fgcolor, lightMode) =>
//            {
//                await ReadFile(file!, delay, fgcolor, lightMode);
//            },
//                fileOption, delayOption, fgcolorOption, lightModeOption);

//            deleteCommand.SetHandler((file, searchTerms) =>
//            {
//                DeleteFromFile(file!, searchTerms);
//            },
//            fileOption, searchTermsOption);

//            addCommand.SetHandler((file, quote, byline) =>
//            {
//                AddToFile(file!, quote, byline);
//            },
//                fileOption, quoteArgument, bylineArgument);

//            await rootCommand.InvokeAsync(args2);
//        };
//    }

//    internal static void DeleteFromFile(FileInfo file, string[] searchTerms)
//    {
//        Console.WriteLine("Deleting from file");
//        File.WriteAllLines(
//            file.FullName, File.ReadLines(file.FullName)
//                .Where(line => searchTerms.All(s => !line.Contains(s))).ToList());
//    }
//    internal static void AddToFile(FileInfo file, string quote, string byline)
//    {
//        Console.WriteLine("Adding to file");
//        using StreamWriter? writer = file.AppendText();
//        writer.WriteLine($"{Environment.NewLine}{Environment.NewLine}{quote}");
//        writer.WriteLine($"{Environment.NewLine}-{byline}");
//        writer.Flush();
//    }

//    internal static async Task ReadFile(
//    FileInfo file, int delay, ConsoleColor fgColor, bool lightMode)
//    {
//        Console.BackgroundColor = lightMode ? ConsoleColor.White : ConsoleColor.Black;
//        Console.ForegroundColor = fgColor;
//        List<string> lines = File.ReadLines(file.FullName).ToList();
//        foreach (string line in lines)
//        {
//            Console.WriteLine(line);
//            await Task.Delay(delay * line.Length);
//        };
//    }
//}

//using System.CommandLine;
//using System.CommandLine.Completions;
//using System.CommandLine.Parsing;

//while (true)
//{
//    var args2 = Console.ReadLine().Split(null);
//    await new DateCommand().InvokeAsync(args2);
//}
//class DateCommand : Command
//{
//    private Argument<string> subjectArgument =
//        new("subject", "The subject of the appointment.");
//    private Option<DateTime> dateOption =
//        new("--date", "The day of week to schedule. Should be within one week.");

//    public DateCommand() : base("schedule", "Makes an appointment for sometime in the next week.")
//    {
//        this.AddArgument(subjectArgument);
//        this.AddOption(dateOption);

//        dateOption.AddCompletions((ctx) => {
//            var today = System.DateTime.Today;
//            var dates = new List<CompletionItem>();
//            foreach (var i in Enumerable.Range(1, 7))
//            {
//                var date = today.AddDays(i);
//                dates.Add(new CompletionItem(
//                    label: date.ToShortDateString(),
//                    sortText: $"{i:2}"));
//            }
//            return dates;
//        });

//        this.SetHandler((subject, date) =>
//        {
//            Console.WriteLine($"Scheduled \"{subject}\" for {date}");
//        },
//            subjectArgument, dateOption);
//    }
//}